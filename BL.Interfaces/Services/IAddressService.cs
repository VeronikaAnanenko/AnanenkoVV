using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Models.Addresses;

namespace BSUIR.BL.Interfaces.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<TMapTo>> GetAddressesAsync<TMapTo>(int? count = null)
            where TMapTo : Address;

        Task<TMapTo> CreateAddressAsync<TMapTo>(Address addressCreate)
            where TMapTo : Address;

        Task<TMapTo> UpdateAddressAsync<TMapTo>(Address addressToUpdate)
            where TMapTo : Address;

        Task<TMapTo> GetAddressByIdAsync<TMapTo>(int id)
            where TMapTo : Address;

        Task<bool> DeleteAddressAsync(int id);
    }
}
