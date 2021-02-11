using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BSUIR.BL.Interfaces.Models.Photos;
using BSUIR.BL.Interfaces.Services;
using BSUIR.Common;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Extensions.Quaryable;
using PhotoDTO = BSUIR.DAL.Interfaces.Models.Photo;

namespace BSUIR.BL.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoContext _photoContext;
        private readonly IMapper _mapper;

        public PhotoService(IMapper mapper, IPhotoContext photoContext)
        {
            _mapper = mapper;
            _photoContext = photoContext;
        }

        public async Task<IEnumerable<TMapTo>> GetRelatedPhotosAsync<TMapTo>(int id,int? count = null) where TMapTo : Photo
        {
            var dbPhotos = await _photoContext.Photos.Take(count).Where(x => x.ItemId == id).ToListAsync();
            var mappedOrders = dbPhotos.Select(p => _mapper.Map<PhotoDTO, TMapTo>(p)).ToList();
            return mappedOrders;
        }

        public async Task<TMapTo> CreatePhotoAsync<TMapTo>(Photo photoCreate) where TMapTo : Photo
        {
            try
            {
                var mappedDtoModel = _mapper.Map<Photo, PhotoDTO>(photoCreate);

                var createdEntity = await _photoContext.Photos.AddAsync(mappedDtoModel);

                if (createdEntity is null)
                {
                    throw new Exception(
                        $"Can not create entity {typeof(PhotoDTO)} from parameter {nameof(photoCreate)}");
                }

                await _photoContext.SaveChangesAsync();

                var mappedCreatedEntity = _mapper.Map<PhotoDTO, TMapTo>(createdEntity);

                return mappedCreatedEntity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeletePhotoAsync(int id)
        {
            try
            {
                var itemToDelete = _photoContext.Photos.First(p => p.Id == id);

                if (itemToDelete is null)
                {
                    throw new Exception($"Can not find order with id {id}");
                }

                var purged = _photoContext.Photos.Remove(itemToDelete);

                var result = await _photoContext.SaveChangesAsync();

                if (result == 0)
                {
                    throw new Exception($"Deleting order with id {id} was not performed");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Some error occured while deleting item with id {id}", ex);
            }
        }
    }
}
