using AutoMapper;
using Skalk.Common.DTO.Order;
using Skalk.DAL.Entities;

namespace Skalk.BLL.MappingProfile
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDTO, Order>();

            CreateMap<Order, OrderDTO>();

            CreateMap<CreateOrderDTO, OrderContractDTO>().ReverseMap();

            CreateMap<ItemShoppingCart, ItemOrder>()
                .ForMember(dest => dest.Mpn, opt => opt.MapFrom(src => src.Mpn))
                .ForMember(dest => dest.OfferId, opt => opt.MapFrom(src => src.OfferId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));

            CreateMap<ItemOrderDTO, ItemOrder>().ReverseMap();
        }
    }
}
