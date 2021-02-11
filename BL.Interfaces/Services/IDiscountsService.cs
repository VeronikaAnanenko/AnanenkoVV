using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Models.Discounts;

namespace BSUIR.BL.Interfaces.Services
{
    public interface IDiscountsService
    {
        Task<IEnumerable<TMapTo>> GetDiscountsAsync<TMapTo>(int? count = null)
            where TMapTo : Discounts;

        Task<TMapTo> CreateDiscountAsync<TMapTo>(Discounts discountCreate)
            where TMapTo : Discounts;

        Task<TMapTo> UpdateDiscountAsync<TMapTo>(Discounts discountToUpdate)
            where TMapTo : Discounts;
        Task<bool> DeleteDiscountAsync(int id);
    }
}
