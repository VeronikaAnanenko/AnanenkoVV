using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Models.Categories;

namespace BSUIR.BL.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<TMapTo>> GetCategoriesAsync<TMapTo>(int? count = null)
            where TMapTo : Category;
        Task<TMapTo> CreateCategoryAsync<TMapTo>(Category categoryCreate)
            where TMapTo : Category;
        Task<bool> DeleteCategoryAsync(int id);
    }
}
