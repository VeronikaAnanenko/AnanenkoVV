using AutoMapper;
using BSUIR.BL.Interfaces.Models.DeliveryAddresses;
using DeliveryAddressDTO = BSUIR.DAL.Interfaces.Models.DeliveryAddress;
namespace BSUIR.BL.MappingProfiles
{
    public class DeliveryAddressMappingProfile : Profile
    {
        public DeliveryAddressMappingProfile()
        {
            this.CreateMap<DeliveryAddress, DeliveryAddressDTO>();
            this.CreateMap<DeliveryAddressDTO, DeliveryAddress>();
        }
    }
}
