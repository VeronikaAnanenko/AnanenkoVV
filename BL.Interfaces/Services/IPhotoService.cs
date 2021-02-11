using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Models.Photos;

namespace BSUIR.BL.Interfaces.Services
{
    public interface IPhotoService
    {
        Task<IEnumerable<TMapTo>> GetRelatedPhotosAsync<TMapTo>(int id,int? count = null)
            where TMapTo : Photo;

        Task<TMapTo> CreatePhotoAsync<TMapTo>(Photo photoCreate)
            where TMapTo : Photo;
        Task<bool> DeletePhotoAsync(int id);
    }
}
