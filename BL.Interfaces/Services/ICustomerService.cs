using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Models.Customers;

namespace BSUIR.BL.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<TMapTo> CreateCustomerAsync<TMapTo>(Customer customerCreate)
            where TMapTo : Customer;

        Task<TMapTo> UpdateCustomerAsync<TMapTo>(Customer customerToUpdate)
            where TMapTo : Customer;

        Task<TMapTo> GetCustomerByIdAsync<TMapTo>(string id)
            where TMapTo : Customer;
    }
}
