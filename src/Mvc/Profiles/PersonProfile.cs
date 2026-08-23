using AutoMapper;
using Contracts.Dtos.Person;
using Mvc.ViewModels;

namespace Mvc.Profiles;

public class PersonProfile : Profile
{
	public PersonProfile()
	{
		CreateMap<PersonViewModel, PersonCreateDto>().ReverseMap();
		CreateMap<PersonReadDto, PersonViewModel>();
		CreateMap<PersonViewModel, PersonReadDto>();
		CreateMap<PersonViewModel, PersonUpdateDto>()
			.ForMember(
				destination => destination.ProfileImageUrl,
				options => options.MapFrom(
					source => source.ProfileImage == null ? null : source.ProfileImage.Url
				)
			);
		CreateMap<PersonUpdateDto, PersonViewModel>()
			.ForMember(
				destination => destination.ProfileImage,
				options => options.MapFrom(
					source => string.IsNullOrWhiteSpace(source.ProfileImageUrl)
						? null
						: new ImageViewModel { Url = source.ProfileImageUrl }
				)
			);
	}
}
