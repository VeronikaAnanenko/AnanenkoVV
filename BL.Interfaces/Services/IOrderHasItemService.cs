using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Models.Items;
using BSUIR.BL.Interfaces.Models.Orders;

namespace BSUIR.BL.Interfaces.Services
{
    public interface IOrderHasItemService
    {
        Task<IEnumerable<TMapTo>> GetOrderHasItemsAsync<TMapTo>(int? count = null)
            where TMapTo : OrderHasItem;

        Task<TMapTo> CreateOrderHasItemAsync<TMapTo>(OrderHasItem itemCreate)
            where TMapTo : OrderHasItem;

    }
}
