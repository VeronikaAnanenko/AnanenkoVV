using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BSUIR.BL.Interfaces.Models.Orders;
using OrderHasItemDTO = BSUIR.DAL.Interfaces.Models.OrderHasItem;

namespace BSUIR.BL.MappingProfiles
{ 
    public class OrderHasItemMappingProfile : Profile
    {
        public OrderHasItemMappingProfile()
        {
            this.CreateMap<OrderHasItem, OrderHasItemDTO>();
            this.CreateMap<OrderHasItemDTO, OrderHasItem>();
        }
    }
}
