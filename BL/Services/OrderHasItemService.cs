using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BSUIR.BL.Interfaces.Models.Items;
using BSUIR.BL.Interfaces.Models.Orders;
using BSUIR.BL.Interfaces.Services;
using BSUIR.Common;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Extensions.Quaryable;
using OrderHasItemDTO = BSUIR.DAL.Interfaces.Models.OrderHasItem;

namespace BSUIR.BL.Services
{
    public class OrderHasItemService : IOrderHasItemService
    {
        private readonly IOrderHasItemContext _orderHasItemContext;
        private readonly IMapper _mapper;

        public OrderHasItemService(IMapper mapper, IOrderHasItemContext orderHasItemContext)
        {
            _mapper = mapper;
            _orderHasItemContext = orderHasItemContext;
        }

        public async Task<IEnumerable<TMapTo>> GetOrderHasItemsAsync<TMapTo>(int? count = null) where TMapTo : OrderHasItem
        {
            var dbOrderHasItems = await _orderHasItemContext.OrderHasItems.Take(count).ToListAsync();
            var mappedOrderHasItems = dbOrderHasItems.Select(p => _mapper.Map<OrderHasItemDTO, TMapTo>(p)).ToList();
            return mappedOrderHasItems;
        }

        public async Task<TMapTo> CreateOrderHasItemAsync<TMapTo>(OrderHasItem itemCreate) where TMapTo : OrderHasItem
        {
            try
            {
                var mappedDtoModel = _mapper.Map<OrderHasItem, OrderHasItemDTO>(itemCreate);

                var createdEntity = _orderHasItemContext.OrderHasItems.Add(mappedDtoModel);

                if (createdEntity is null)
                {
                    throw new Exception(
                        $"Can not create entity {typeof(OrderHasItemDTO)} from parameter {nameof(itemCreate)}");
                }

                await _orderHasItemContext.SaveChangesAsync();

                var mappedCreatedEntity = _mapper.Map<OrderHasItemDTO, TMapTo>(createdEntity);

                return mappedCreatedEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
