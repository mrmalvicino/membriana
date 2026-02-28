using AutoMapper;
using Contracts.Dtos.Payment;
using Mvc.Areas.Admin.ViewModels;

namespace Mvc.Profiles;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        #region Create
        CreateMap<PaymentViewModel, PaymentCreateDto>().ReverseMap();
        #endregion

        #region Read
        CreateMap<PaymentReadDto, PaymentViewModel>().ReverseMap();
        #endregion

        #region Update
        CreateMap<PaymentViewModel, PaymentUpdateDto>().ReverseMap();
        #endregion
    }
}
