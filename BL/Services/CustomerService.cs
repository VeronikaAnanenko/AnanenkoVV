using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BSUIR.BL.Interfaces.Models.Customers;
using BSUIR.BL.Interfaces.Services;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Extensions.Quaryable;
using CustomerDTO = BSUIR.DAL.Interfaces.Models.Customer;

namespace BSUIR.BL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomersContext _customersContext;
        private readonly IMapper _mapper;

        public CustomerService(IMapper mapper, ICustomersContext customersContext)
        {
            _mapper = mapper;
            _customersContext = customersContext;
        }

        public async Task<TMapTo> CreateCustomerAsync<TMapTo>(Customer customerCreate) where TMapTo : Customer
        {
            try
            {
                var mappedDtoModel = _mapper.Map<Customer, CustomerDTO>(customerCreate);

                var createdEntity = await _customersContext.Customers.AddAsync(mappedDtoModel);

                if (createdEntity is null)
                {
                    throw new Exception(
                        $"Can not create entity {typeof(CustomerDTO)} from parameter {nameof(customerCreate)}");
                }

                await _customersContext.SaveChangesAsync();

                var mappedCreatedEntity = _mapper.Map<CustomerDTO, TMapTo>(createdEntity);

                return mappedCreatedEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TMapTo> UpdateCustomerAsync<TMapTo>(Customer customerToUpdate) where TMapTo : Customer
        {
            try
            {
                var address = await this.GetCustomerByIdAsync(customerToUpdate.Id);

                var mappedDtoModel = _mapper.Map<Customer, CustomerDTO>(customerToUpdate, address);

                var updatedModel = _customersContext.Customers.Update(mappedDtoModel);

                if (updatedModel is null)
                {
                    throw new Exception(
                        $"Can not update entity {typeof(CustomerDTO)} using data from object parameter {nameof(customerToUpdate)}");
                }

                await _customersContext.SaveChangesAsync();

                return _mapper.Map<TMapTo>(updatedModel);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Some error occured while updating customer. Updated changes won't be applied", ex);
            }
        }

        public async Task<TMapTo> GetCustomerByIdAsync<TMapTo>(string id) where TMapTo : Customer
        {
            var foundCustomer = await this.GetCustomerByIdAsync(id);

            return _mapper.Map<TMapTo>(foundCustomer);
        }
        private async Task<CustomerDTO> GetCustomerByIdAsync(string customerId)
        {
            if (!_customersContext.Customers.Any(p => p.Id == customerId))
            {
                throw new Exception($"Customer with id: {customerId} not found.");
            }

            var addressWithCorrespondedId =
                await _customersContext.Customers.Where(p => p.Id == customerId).ToArrayAsync();

            if (addressWithCorrespondedId.Length > 1)
            {
                throw new Exception(
                    $"More than one entity of type {typeof(CustomerDTO)} with id {customerId} where found");
            }

            var foundAddress = addressWithCorrespondedId[0];

            return foundAddress;
        }
    }
}
