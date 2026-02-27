using Contracts.Dtos.Image;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageReadDto>();
        }
    }
}
