using AutoMapper;
using Mvc.Dtos.Image;
using Mvc.Models;

namespace Mvc.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<ImageReadDto, ImageViewModel>().ReverseMap();
        }
    }
}
