using AutoMapper;
using BSUIR.BL.Interfaces.Models.Orders;
using OrderDTO = BSUIR.DAL.Interfaces.Models.Order;
namespace BSUIR.BL.MappingProfiles
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            this.CreateMap<Order,OrderDTO>();
            this.CreateMap<OrderDTO,Order>();
        }
    }
}
