using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BSUIR.DAL.Interfaces;
using BSUIR.DAL.Interfaces.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BSUIR.DAL.Context
{
    public abstract class AppDomainContextBase<TDbContext>
        where TDbContext : IdentityDbContext<User>
    {
        private readonly TDbContext _dbContext;

        public AppDomainContextBase(TDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
            await _dbContext.SaveChangesAsync(cancellationToken);

        protected IEntitySet<TEntityType> GetEntitySet<TEntityType>()
            where TEntityType : class
        {
            var entitySetConstructedType = typeof(EntitySet<>).MakeGenericType(typeof(TEntityType));

            var dbSetGenericPropertyValue = TypeDescriptor
                .GetProperties(_dbContext)
                .Cast<PropertyDescriptor>()
                .Where(p =>
                    p.PropertyType.IsConstructedGenericType && p.PropertyType.GetGenericTypeDefinition() ==
                                                            typeof(DbSet<>)
                                                            && p.PropertyType.GenericTypeArguments[0] ==
                                                            typeof(TEntityType)).FirstOrDefault().GetValue(_dbContext);

            var entitySet = Activator.CreateInstance(entitySetConstructedType, dbSetGenericPropertyValue);

            return entitySet as IEntitySet<TEntityType>;
        }
    }
}
