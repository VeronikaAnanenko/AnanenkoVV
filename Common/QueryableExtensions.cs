using System.Linq;

namespace BSUIR.Common
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Take<TEntity>(this IQueryable<TEntity> source, int? amountToTake)
        {
            if (!amountToTake.HasValue)
            {
                return source;
            }

            return Queryable.Take(source, amountToTake.Value);
        }
    }
}
