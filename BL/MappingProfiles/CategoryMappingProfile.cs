using AutoMapper;
using BSUIR.BL.Interfaces.Models.Categories;
using CategoryDTO = BSUIR.DAL.Interfaces.Models.Category;

namespace BSUIR.BL.MappingProfiles
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            this.CreateMap<Category, CategoryDTO>();
            this.CreateMap<CategoryDTO, Category>();
        }
    }
}
