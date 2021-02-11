using AutoMapper;
using BSUIR.BL.Interfaces.Models.Discounts;
using DiscountsDTO = BSUIR.DAL.Interfaces.Models.Discounts;
namespace BSUIR.BL.MappingProfiles
{
    public class DiscountsMappingProfile : Profile
    {
        public DiscountsMappingProfile()
        {
            this.CreateMap<Discounts, DiscountsDTO>();
            this.CreateMap<DiscountsDTO, Discounts>();
        }
    }
}
