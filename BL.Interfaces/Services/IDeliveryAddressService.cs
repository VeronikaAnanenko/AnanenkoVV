using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Models.DeliveryAddresses;

namespace BSUIR.BL.Interfaces.Services
{
    public interface IDeliveryAddressService
    {
        Task<IEnumerable<TMapTo>> GetDeliveryAddressesAsync<TMapTo>(int? count = null)
            where TMapTo : DeliveryAddress;
    }
}
