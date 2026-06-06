using AutoMapper;
using Shared.Contracts.DTOs.Review;
using ReviewEntity = Review.Domain.Entities.Review;

namespace Review.Application.Mappings
{
    public class ReviewMappings : Profile
    {
        public ReviewMappings()
        {
            CreateMap<ReviewEntity, ReviewDto>()
                .ForMember(dest => dest.ReviewerType,
                           opt => opt.MapFrom(src => src.ReviewerType.ToString()));
        }
    }
}
