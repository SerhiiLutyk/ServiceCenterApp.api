using AutoMapper;
using ServiceCenterAppBLL.DTO.AdditionalServiceDto;
// DTO namespaces
using ServiceCenterAppBLL.DTO.ClientDto;
using ServiceCenterAppBLL.DTO.OrderDto;
using ServiceCenterAppBLL.DTO.PaymentDto;
using ServiceCenterAppBLL.DTO.RepairTypeDto;
using ServiceCenterAppDalEF.Entities;
using ServiceCenterAppBLL.DTO.AdditionalServiceDto;
using ServiceCenterAppBLL.DTO.ClientDto;
using ServiceCenterAppBLL.DTO.OrderDto;
using ServiceCenterAppBLL.DTO.PaymentDto;
using ServiceCenterAppBLL.DTO.RepairTypeDto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServiceCenterAppBLL.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<Client, ClientResponseDto>();
            CreateMap<ClientCreateDto, Client>();
            CreateMap<ClientUpdateDto, Client>();


            CreateMap<Order, OrderResponseDto>();
            CreateMap<OrderCreateDto, Order>()
                .ForMember(d => d.OrderDate, opt => opt.Ignore())
                .ForMember(d => d.Status, opt => opt.Ignore());
            CreateMap<OrderUpdateDto, Order>();

    
            CreateMap<RepairType, RepairTypeResponseDto>();
            CreateMap<RepairTypeCreateDto, RepairType>();
            CreateMap<RepairTypeUpdateDto, RepairType>();


            CreateMap<AdditionalService, AdditionalServiceResponseDto>();
            CreateMap<AdditionalServiceCreateDto, AdditionalService>();
            CreateMap<AdditionalServiceUpdateDto, AdditionalService>();

            CreateMap<Payment, PaymentResponseDto>();
            CreateMap<PaymentCreateDto, Payment>()
                .ForMember(d => d.PaymentDate, opt => opt.Ignore());
            CreateMap<PaymentUpdateDto, Payment>();
        }
    }
}
