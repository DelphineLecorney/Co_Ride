using AutoMapper;
using Identity.Application.Commands.Login;
using Identity.Application.Commands.Register;
using Identity.Application.DTOs;
using Identity.Domain.Entities;
using Shared.Contracts.DTOs.Identity;

namespace Identity.Application.Mappings
{
    /// <summary>
    /// C'est la classe de configuration AutoMapper pour l'application Identity. 
    /// Elle définit les profils de mappage entre les entités du domaine et les 
    /// DTOs utilisés dans les différentes couches de l'application.
    /// </summary>
    public class IdentityMapping : Profile
    {
        public IdentityMapping()
        {
            CreateMap<ApplicationUser, UserInternalDto>();
            CreateMap<ApplicationUser, UserDto>()
            .ForCtorParam("Roles", opt => opt.MapFrom((src, ctx) =>
            {
                if (ctx.Items.TryGetValue("Roles", out var rolesObj) && rolesObj is List<string> roles)
                {
                    return roles;
                }
                return new List<string>();
            }));

            CreateMap<ApplicationUser, AuthResponse>()
            .ForMember(dest => dest.User.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<RegisterCommand, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<LoginRequest, LoginCommand>();
            CreateMap<RegisterRequest, RegisterCommand>();
        }
    }
}
