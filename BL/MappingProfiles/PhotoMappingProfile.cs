using AutoMapper;
using BSUIR.BL.Interfaces.Models.Photos;
using PhotoDTO = BSUIR.DAL.Interfaces.Models.Photo;
namespace BSUIR.BL.MappingProfiles
{
    public class PhotoMappingProfile : Profile
    {
        public PhotoMappingProfile()
        {
            this.CreateMap<Photo,PhotoDTO>();
            this.CreateMap<PhotoDTO, Photo>();
        }
    }
}
