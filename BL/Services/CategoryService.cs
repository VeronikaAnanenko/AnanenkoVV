using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BSUIR.BL.Interfaces.Models.Categories;
using BSUIR.BL.Interfaces.Services;
using BSUIR.Common;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Extensions.Quaryable;
using CategoryDTO = BSUIR.DAL.Interfaces.Models.Category;

namespace BSUIR.BL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryContext _categoryContext;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, ICategoryContext categoryContext)
        {
            _mapper = mapper;
            _categoryContext = categoryContext;
        }

        public async Task<IEnumerable<TMapTo>> GetCategoriesAsync<TMapTo>(int? count = null) where TMapTo : Category
        {
            var dbAddresses = await _categoryContext.Categories.Take(count).ToListAsync();
            var mappedCategories = dbAddresses.Select(p => _mapper.Map<CategoryDTO, TMapTo>(p)).ToList();
            return mappedCategories;
        }

        public async Task<TMapTo> CreateCategoryAsync<TMapTo>(Category categoryCreate) where TMapTo : Category
        {
            try
            {
                var mappedDtoModel = _mapper.Map<Category, CategoryDTO>(categoryCreate);

                var createdEntity = await _categoryContext.Categories.AddAsync(mappedDtoModel);

                if (createdEntity is null)
                {
                    throw new Exception(
                        $"Can not create entity {typeof(CategoryDTO)} from parameter {nameof(categoryCreate)}");
                }

                await _categoryContext.SaveChangesAsync();

                var mappedCreatedEntity = _mapper.Map<CategoryDTO, TMapTo>(createdEntity);

                return mappedCreatedEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var categoryToDelete = _categoryContext.Categories.First(p => p.Id == id);

                if (categoryToDelete is null)
                {
                    throw new Exception($"Can not find category with id {id}");
                }

                var purged = _categoryContext.Categories.Remove(categoryToDelete);

                var result = await _categoryContext.SaveChangesAsync();

                if (result == 0)
                {
                    throw new Exception($"Deleting category with id {id} was not performed");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Some error occured while deleting category with id {id}", ex);
            }
        }
    }
}
