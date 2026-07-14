using AutoMapper;
using Contracts.Dtos.Image;
using Mvc.ViewModels;

namespace Mvc.Profiles;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<ImageReadDto, ImageViewModel>().ReverseMap();
    }
}
