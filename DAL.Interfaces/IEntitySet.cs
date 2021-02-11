using System.Linq;
using System.Threading.Tasks;

namespace BSUIR.DAL.Interfaces
{
    public interface IEntitySet<TEntity> : IQueryable<TEntity>
      where TEntity : class
    {
        TEntity Add(TEntity entity);

        Task<TEntity> AddAsync(TEntity entityToAdd);

        TEntity Remove(TEntity entity);

        TEntity Update(TEntity entity);

    }
}
