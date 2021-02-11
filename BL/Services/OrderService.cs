using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BSUIR.BL.Interfaces.Services;
using BSUIR.Common;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Extensions.Quaryable;
using Item = BSUIR.BL.Interfaces.Models.Items.Item;
using Order = BSUIR.BL.Interfaces.Models.Orders.Order;
using OrderDTO = BSUIR.DAL.Interfaces.Models.Order;

namespace BSUIR.BL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrdersContext _ordersContext;
        private readonly IMapper _mapper;

        public OrderService(IMapper mapper, IOrdersContext ordersContext)
        {
            _mapper = mapper;
            _ordersContext = ordersContext;
        }

        public async Task<IEnumerable<TMapTo>> GetRelatedOrderAsync<TMapTo>(string id, int? count = null) where TMapTo : Order
        {
            var dbOrders = await _ordersContext.Orders.Take(count).Where(x=>x.CustomerId==id).ToListAsync();
            var mappedOrders = dbOrders.Select(p => _mapper.Map<OrderDTO, TMapTo>(p)).ToList();
            return mappedOrders;
        }

        public async Task<TMapTo> CreateOrderAsync<TMapTo>(Order orderCreate) where TMapTo : Order
        {
            try
            {
                var mappedDtoModel = _mapper.Map<Order, OrderDTO>(orderCreate);
                var createdEntity = await _ordersContext.Orders.AddAsync(mappedDtoModel);

                if (createdEntity is null)
                {
                    throw new Exception(
                        $"Can not create entity {typeof(OrderDTO)} from parameter {nameof(orderCreate)}");
                }

                await _ordersContext.SaveChangesAsync();

                var mappedCreatedEntity = _mapper.Map<OrderDTO, TMapTo>(createdEntity);

                return mappedCreatedEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TMapTo> UpdateOrderAsync<TMapTo>(Order orderToUpdate) where TMapTo : Order
        {
            try
            {
                var address = await this.GetOrderByIdAsync(orderToUpdate.Id);

                var mappedDtoModel = _mapper.Map<Order, OrderDTO>(orderToUpdate, address);

                var updatedModel = _ordersContext.Orders.Update(mappedDtoModel);

                if (updatedModel is null)
                {
                    throw new Exception(
                        $"Can not update entity {typeof(OrderDTO)} using data from object parameter {nameof(orderToUpdate)}");
                }

                await _ordersContext.SaveChangesAsync();

                return _mapper.Map<TMapTo>(updatedModel);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Some error occured while updating order. Updated changes won't be applied", ex);
            }
        }

        public async Task<TMapTo> GetOrderByIdAsync<TMapTo>(int id) where TMapTo : Order
        {
            var foundOrder = await this.GetOrderByIdAsync(id);
            return _mapper.Map<TMapTo>(foundOrder);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            try
            {
                var itemToDelete = _ordersContext.Orders.First(p => p.Id == id);

                if (itemToDelete is null)
                {
                    throw new Exception($"Can not find order with id {id}");
                }

                var purged = _ordersContext.Orders.Remove(itemToDelete);

                var result = await _ordersContext.SaveChangesAsync();

                if (result == 0)
                {
                    throw new Exception($"Deleting order with id {id} was not performed");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Some error occured while deleting item with id {id}", ex);
            }
        }
        private async Task<OrderDTO> GetOrderByIdAsync(int orderId)
        {
            if (!_ordersContext.Orders.Any(p => p.Id == orderId))
            {
                throw new Exception($"Order with id: {orderId} not found.");
            }

            var addressWithCorrespondedId =
                await _ordersContext.Orders.Where(p => p.Id == orderId).ToArrayAsync();

            if (addressWithCorrespondedId.Length > 1)
            {
                throw new Exception(
                    $"More than one entity of type {typeof(OrderDTO)} with id {orderId} where found");
            }

            var foundAddress = addressWithCorrespondedId[0];

            return foundAddress;
        }
    }
}
