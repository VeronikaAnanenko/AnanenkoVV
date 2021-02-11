using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BSUIR.BL.Interfaces.Models.Items;
using BSUIR.BL.Interfaces.Services;
using BSUIR.Common;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Extensions.Quaryable;
using ItemDTO = BSUIR.DAL.Interfaces.Models.Item;

namespace BSUIR.BL.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemsContext _itemsContext;
        private readonly IMapper _mapper;

        public ItemService(IMapper mapper, IItemsContext itemsContext)
        {
            _mapper = mapper;
            _itemsContext = itemsContext;
        }

        public async Task<IEnumerable<TMapTo>> GetItemsAsync<TMapTo>(int? count = null) where TMapTo : Item
        {
            var dbAddresses = await _itemsContext.Items.Take(count).ToListAsync();
            var mappedAddresses = dbAddresses.Select(p => _mapper.Map<ItemDTO, TMapTo>(p)).ToList();
            return mappedAddresses;
        }

        public async Task<TMapTo> CreateItemAsync<TMapTo>(Item itemCreate) where TMapTo : Item
        {
            try
            {
                var mappedDtoModel = _mapper.Map<Item, ItemDTO>(itemCreate);

                var createdEntity = await _itemsContext.Items.AddAsync(mappedDtoModel);

                if (createdEntity is null)
                {
                    throw new Exception(
                        $"Can not create entity {typeof(ItemDTO)} from parameter {nameof(itemCreate)}");
                }

                await _itemsContext.SaveChangesAsync();

                var mappedCreatedEntity = _mapper.Map<ItemDTO, TMapTo>(createdEntity);

                return mappedCreatedEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TMapTo> UpdateItemAsync<TMapTo>(Item itemToUpdate) where TMapTo : Item
        {
            try
            {
                var address = await this.GetItemByIdAsync(itemToUpdate.Id);

                var mappedDtoModel = _mapper.Map<Item, ItemDTO>(itemToUpdate, address);

                var updatedModel = _itemsContext.Items.Update(mappedDtoModel);

                if (updatedModel is null)
                {
                    throw new Exception(
                        $"Can not update entity {typeof(ItemDTO)} using data from object parameter {nameof(itemToUpdate)}");
                }

                await _itemsContext.SaveChangesAsync();

                return _mapper.Map<TMapTo>(updatedModel);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Some error occured while updating order. Updated changes won't be applied", ex);
            }
        }

        public async Task<TMapTo> GetItemByIdAsync<TMapTo>(int id) where TMapTo : Item
        {
            var foundItem = await this.GetItemByIdAsync(id);

            return _mapper.Map<TMapTo>(foundItem);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            try
            {
                var itemToDelete = _itemsContext.Items.First(p => p.Id == id);

                if (itemToDelete is null)
                {
                    throw new Exception($"Can not find item with id {id}");
                }

                var purged = _itemsContext.Items.Remove(itemToDelete);

                var result = await _itemsContext.SaveChangesAsync();

                if (result == 0)
                {
                    throw new Exception($"Deleting item with id {id} was not performed");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Some error occured while deleting item with id {id}", ex);
            }
        }
        private async Task<ItemDTO> GetItemByIdAsync(int itemId)
        {
            if (!_itemsContext.Items.Any(p => p.Id == itemId))
            {
                throw new Exception($"Item with id: {itemId} not found.");
            }

            var addressWithCorrespondedId =
                await _itemsContext.Items.Where(p => p.Id == itemId).ToArrayAsync();

            if (addressWithCorrespondedId.Length > 1)
            {
                throw new Exception(
                    $"More than one entity of type {typeof(ItemDTO)} with id {itemId} where found");
            }

            var foundAddress = addressWithCorrespondedId[0];

            return foundAddress;
        }
    }
}
