using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Models.Items;
using BSUIR.BL.Interfaces.Models.Orders;

namespace BSUIR.BL.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<TMapTo>> GetRelatedOrderAsync<TMapTo>( string id,int? count = null)
            where TMapTo : Order;

        Task<TMapTo> CreateOrderAsync<TMapTo>(Order orderCreate)
            where TMapTo : Order;

        Task<TMapTo> UpdateOrderAsync<TMapTo>(Order orderToUpdate)
            where TMapTo : Order;

        Task<TMapTo> GetOrderByIdAsync<TMapTo>(int id)
            where TMapTo : Order;

        Task<bool> DeleteOrderAsync(int id);
    }
}
