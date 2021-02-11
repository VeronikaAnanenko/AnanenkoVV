using AutoMapper;
using BSUIR.BL.Interfaces.Models.Items;
using ItemDTO = BSUIR.DAL.Interfaces.Models.Item;
namespace BSUIR.BL.MappingProfiles
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile()
        {
            this.CreateMap<Item, ItemDTO>();
            this.CreateMap<ItemDTO, Item>();
        }
    }
}
