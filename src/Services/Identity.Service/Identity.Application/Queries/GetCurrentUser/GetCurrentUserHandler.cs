using AutoMapper;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Contracts.DTOs.Identity;

namespace Identity.Application.Queries.GetCurrentUser
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public GetCurrentUserHandler(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user == null)
                throw new Exception("Utilisateur non trouvé");

            var roles = await _userManager.GetRolesAsync(user);

            return _mapper.Map<UserDto>(user, opt =>
            {
                opt.Items["Roles"] = roles.ToList();
            });
        }
    }

}
