using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Models.Items;

namespace BSUIR.BL.Interfaces.Services
{
    public interface IItemService
    {
        Task<IEnumerable<TMapTo>> GetItemsAsync<TMapTo>(int? count = null)
            where TMapTo : Item;

        Task<TMapTo> CreateItemAsync<TMapTo>(Item itemCreate)
            where TMapTo : Item;

        Task<TMapTo> UpdateItemAsync<TMapTo>(Item itemToUpdate)
            where TMapTo : Item;

        Task<TMapTo> GetItemByIdAsync<TMapTo>(int id)
            where TMapTo : Item;
        Task<bool> DeleteItemAsync(int id);
    }
}
