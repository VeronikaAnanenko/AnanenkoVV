using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BSUIR.BL.Interfaces.Models.DeliveryAddresses;
using BSUIR.BL.Interfaces.Services;
using BSUIR.Common;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Extensions.Quaryable;
using DeliveryAddressDTO = BSUIR.DAL.Interfaces.Models.DeliveryAddress;

namespace BSUIR.BL.Services
{
    public class DeliveryAddressService : IDeliveryAddressService
    {
        private readonly IDeliveryAddressesContext _deliveryAddressesContext;
        private readonly IMapper _mapper;

        public DeliveryAddressService(IMapper mapper, IDeliveryAddressesContext deliveryAddressesContext)
        {
            _mapper = mapper;
            _deliveryAddressesContext = deliveryAddressesContext;
        }

        public async Task<IEnumerable<TMapTo>> GetDeliveryAddressesAsync<TMapTo>(int? count = null) where TMapTo : DeliveryAddress
        {
            var dbAddresses = await _deliveryAddressesContext.DeliveryAddresses.Take(count).ToListAsync();
            var mappedAddresses = dbAddresses.Select(p => _mapper.Map<DeliveryAddressDTO, TMapTo>(p)).ToList();
            return mappedAddresses;
        }
    }
}
