using BSUIR.DAL.Interfaces;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Models;

namespace BSUIR.DAL.Context
{
    public class ItemsContext : AppDomainContextBase<InternetShopContext>, IItemsContext
    {
        public ItemsContext(InternetShopContext dbContext) : base(dbContext)
        {
        }

        public IEntitySet<Item> Items => base.GetEntitySet<Item>();
    }
}
