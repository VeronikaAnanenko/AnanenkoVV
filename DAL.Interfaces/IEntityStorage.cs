using System.Threading;
using System.Threading.Tasks;

namespace BSUIR.DAL.Interfaces
{
    public interface IEntityStorage
    {
        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
