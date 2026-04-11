using AutoMapper;
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
            // CreateMap<Source, Destination>();
            CreateMap<ApplicationUser, UserInternalDto>();

            CreateMap<ApplicationUser, UserDto>();

            CreateMap<RegisterCommand, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<ApplicationUser, AuthResultDto>();
        }
    }
}
