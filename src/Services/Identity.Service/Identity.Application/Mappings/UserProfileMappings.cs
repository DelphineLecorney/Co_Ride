using AutoMapper;
using Identity.Domain.Entities;
using Shared.Contracts.Profile;

namespace Identity.Application.Mappings
{
    public class UserProfileMappings : Profile
    {
        public UserProfileMappings()
        {
            CreateMap<UserProfile, UserProfileDto>();
        }
    }
}
