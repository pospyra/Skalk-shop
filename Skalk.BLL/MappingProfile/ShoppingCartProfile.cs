using AutoMapper;
using Skalk.Common.DTO.ShoppingCart;
using Skalk.DAL.Entities;

namespace Skalk.BLL.MappingProfile
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile() 
        {
            CreateMap<ItemCartDTO, ItemShoppingCart>().ReverseMap();
            CreateMap<NewItemCartDTO, ItemShoppingCart>().ReverseMap();

            CreateMap<ShoppingCart, ShoppingCartDTO>().ReverseMap();
        }
    }
}
