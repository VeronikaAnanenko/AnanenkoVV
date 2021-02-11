using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BSUIR.BL.Interfaces.Models.Addresses;
using AddressDTO = BSUIR.DAL.Interfaces.Models.Address;
namespace BL.MappingProfiles
{
    public class AddressMappingProfile : Profile
    {
        public AddressMappingProfile()
        {
            this.CreateMap<Address, AddressDTO>();
            this.CreateMap<AddressDTO, Address>();
        }
    }
}
