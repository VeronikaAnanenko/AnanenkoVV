using AutoMapper;
using BSUIR.BL.Interfaces.Models.Customers;
using CustomerDTO = BSUIR.DAL.Interfaces.Models.Customer;
namespace BSUIR.BL.MappingProfiles
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            this.CreateMap<Customer, CustomerDTO>();
            this.CreateMap<CustomerDTO, Customer>();
        }
    }
}
