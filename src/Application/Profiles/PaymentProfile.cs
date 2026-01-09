using Application.Dtos.Payment;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            #region Create
            CreateMap<PaymentCreateDto, Payment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Member, opt => opt.Ignore())
                .ForMember(dest => dest.Organization, opt => opt.Ignore())
                .ForMember(dest => dest.OrganizationId, opt => opt.Ignore());
            #endregion

            #region Read
            CreateMap<Payment, PaymentReadDto>()
                .ForMember(dest => dest.Member, opt => opt.MapFrom(src => src.Member));
            #endregion

            #region Update
            CreateMap<PaymentUpdateDto, Payment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Member, opt => opt.Ignore())
                .ForMember(dest => dest.MemberId, opt => opt.Ignore()) // Un pago no se puede transferir entre miembros
                .ForMember(dest => dest.Organization, opt => opt.Ignore())
                .ForMember(dest => dest.OrganizationId, opt => opt.Ignore()); // Por seguridad, no se toma desde el cliente
            #endregion
        }
    }
}
