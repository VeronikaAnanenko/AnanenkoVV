using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BSUIR.BL.Interfaces.Models.Discounts;
using BSUIR.BL.Interfaces.Services;
using BSUIR.Common;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Extensions.Quaryable;
using DiscountsDTO = BSUIR.DAL.Interfaces.Models.Discounts;

namespace BSUIR.BL.Services
{
    public class DiscountsService : IDiscountsService
    {
        private readonly IDiscountsContext _discountsContext;
        private readonly IMapper _mapper;

        public DiscountsService(IDiscountsContext discountsContext, IMapper mapper)
        {
            _discountsContext = discountsContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TMapTo>> GetDiscountsAsync<TMapTo>(int? count = null) where TMapTo : Discounts
        {
            var dbAddresses = await _discountsContext.Discounts.Take(count).ToListAsync();
            var mappedAddresses = dbAddresses.Select(p => _mapper.Map<DiscountsDTO, TMapTo>(p)).ToList();
            return mappedAddresses;

        }

        public async Task<TMapTo> CreateDiscountAsync<TMapTo>(Discounts discountCreate) where TMapTo : Discounts
        {
            try
            {
                var mappedDtoModel = _mapper.Map<Discounts, DiscountsDTO>(discountCreate);

                var createdEntity = await _discountsContext.Discounts.AddAsync(mappedDtoModel);

                if (createdEntity is null)
                {
                    throw new Exception(
                        $"Can not create entity {typeof(DiscountsDTO)} from parameter {nameof(discountCreate)}");
                }

                await _discountsContext.SaveChangesAsync();

                var mappedCreatedEntity = _mapper.Map<DiscountsDTO, TMapTo>(createdEntity);

                return mappedCreatedEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TMapTo> UpdateDiscountAsync<TMapTo>(Discounts discountToUpdate) where TMapTo : Discounts
        {
            try
            {
                var address = await this.GetDiscountByIdAsync(discountToUpdate.Id);

                var mappedDtoModel = _mapper.Map<Discounts, DiscountsDTO>(discountToUpdate, address);

                var updatedModel = _discountsContext.Discounts.Update(mappedDtoModel);

                if (updatedModel is null)
                {
                    throw new Exception(
                        $"Can not update entity {typeof(DiscountsDTO)} using data from object parameter {nameof(discountToUpdate)}");
                }

                await _discountsContext.SaveChangesAsync();

                return _mapper.Map<TMapTo>(updatedModel);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Some error occured while updating customer. Updated changes won't be applied", ex);
            }
        }

        public async Task<bool> DeleteDiscountAsync(int id)
        {
            try
            {
                var discountToDelete = _discountsContext.Discounts.First(p => p.Id == id);

                if (discountToDelete is null)
                {
                    throw new Exception($"Can not find discount with id {id}");
                }

                var purged = _discountsContext.Discounts.Remove(discountToDelete);

                var result = await _discountsContext.SaveChangesAsync();

                if (result == 0)
                {
                    throw new Exception($"Deleting discount with id {id} was not performed");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Some error occured while deleting discount with id {id}", ex);
            }
        }
        private async Task<DiscountsDTO> GetDiscountByIdAsync(int discountId)
        {
            if (!_discountsContext.Discounts.Any(p => p.Id == discountId))
            {
                throw new Exception($"Discount with id: {discountId} not found.");
            }

            var addressWithCorrespondedId =
                await _discountsContext.Discounts.Where(p => p.Id == discountId).ToArrayAsync();

            if (addressWithCorrespondedId.Length > 1)
            {
                throw new Exception(
                    $"More than one entity of type {typeof(DiscountsDTO)} with id {discountId} where found");
            }

            var foundAddress = addressWithCorrespondedId[0];

            return foundAddress;
        }
    }
}
