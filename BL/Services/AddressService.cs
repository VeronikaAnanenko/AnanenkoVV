using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BSUIR.BL.Interfaces.Services;
using BSUIR.BL.Interfaces.Models.Addresses;
using BSUIR.Common;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Extensions.Quaryable;
using AddressDTO = BSUIR.DAL.Interfaces.Models.Address;

namespace BSUIR.BL.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressesContext _addressesContext;
        private readonly IMapper _mapper;

        public AddressService(IAddressesContext addressesContext, IMapper mapper)
        {
            this._addressesContext = addressesContext;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<TMapTo>> GetAddressesAsync<TMapTo>(int? count = null) where TMapTo : Address
        {
            var dbAddresses= await _addressesContext.Addresses.Take(count).ToListAsync();
            var mappedAddresses = dbAddresses.Select(p=>_mapper.Map<AddressDTO,TMapTo>(p)).ToList();
            return mappedAddresses;
        }

        public async Task<TMapTo> CreateAddressAsync<TMapTo>(Address addressCreate) where TMapTo : Address
        {
            try
            {
                var mappedDtoModel = _mapper.Map<Address, AddressDTO>(addressCreate);

                var createdEntity = await _addressesContext.Addresses.AddAsync(mappedDtoModel);

                if (createdEntity is null)
                {
                    throw new Exception(
                        $"Can not create entity {typeof(AddressDTO)} from parameter {nameof(addressCreate)}");
                }

                await _addressesContext.SaveChangesAsync();

                var mappedCreatedEntity = _mapper.Map<AddressDTO, TMapTo>(createdEntity);

                return mappedCreatedEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TMapTo> UpdateAddressAsync<TMapTo>(Address addressToUpdate) where TMapTo : Address
        {
            try
            {
                var address = await this.GetAddressByIdAsync(addressToUpdate.Id);

                var mappedDtoModel = _mapper.Map<Address, AddressDTO>(addressToUpdate, address);

                var updatedModel = _addressesContext.Addresses.Update(mappedDtoModel);

                if (updatedModel is null)
                {
                    throw new Exception(
                        $"Can not update entity {typeof(AddressDTO)} using data from object parameter {nameof(addressToUpdate)}");
                }

                await _addressesContext.SaveChangesAsync();

                return _mapper.Map<TMapTo>(updatedModel);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Some error occured while updating address. Updated changes won't be applied", ex);
            }
        }

        public async Task<TMapTo> GetAddressByIdAsync<TMapTo>(int id) where TMapTo : Address
        {
            var foundAddress = await this.GetAddressByIdAsync(id);

            return _mapper.Map<TMapTo>(foundAddress);
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            try
            {
                var addressToDelete = _addressesContext.Addresses.First(p => p.Id== id);

                if (addressToDelete is null)
                {
                    throw new Exception($"Can not find address with id {id}");
                }

                var purged = _addressesContext.Addresses.Remove(addressToDelete);

                var result = await _addressesContext.SaveChangesAsync();

                if (result == 0)
                {
                    throw new Exception($"Deleting address with id {id} was not performed");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Some error occured while deleting address with id {id}", ex);
            }
        }
        private async Task<AddressDTO> GetAddressByIdAsync(int addressId)
        {
            if (!_addressesContext.Addresses.Any(p => p.Id == addressId))
            {
                throw new Exception($"Address with id: {addressId} not found.");
            }

            var addressWithCorrespondedId =
                await _addressesContext.Addresses.Where(p => p.Id == addressId).ToArrayAsync();

            if (addressWithCorrespondedId.Length > 1)
            {
                throw new Exception(
                    $"More than one entity of type {typeof(AddressDTO)} with id {addressId} where found");
            }

            var foundAddress = addressWithCorrespondedId[0];

            return foundAddress;
        }
    }
}
