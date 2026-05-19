using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Contracts.DTOs.Identity;
using Shared.Contracts.Profile;

namespace Identity.Application.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenService _jwtService;
        private readonly IUserProfileRepository _profileRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public RegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtTokenService jwtService,
            IUserProfileRepository profileRepository,
            IPublishEndpoint publishEndpoint)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _profileRepository = profileRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<AuthResponse> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            var profile = user.InitializeProfile();

            await _profileRepository.AddAsync(profile, cancellationToken);
            await _profileRepository.SaveChangesAsync(cancellationToken);

            await _publishEndpoint.Publish(new UserProfileUpdatedEvent(
                user.Id,
                profile.FirstName,
                profile.LastName,
                profile.AvatarUrl,
                profile.UpdatedAt
            ), cancellationToken);

            var token = _jwtService.GenerateToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                roles.ToList(),
                user.IsEmailVerified,
                user.ReputationScore,
                user.ReviewCount,
                user.CreatedAt
            );

            return new AuthResponse(
                AccessToken: token.AccessToken,
                RefreshToken: token.RefreshToken,
                ExpiresAt: token.ExpiresAt,
                User: userDto
            );
        }
    }
}