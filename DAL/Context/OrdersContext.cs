using BSUIR.DAL.Interfaces;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Models;

namespace BSUIR.DAL.Context
{
    public class OrdersContext : AppDomainContextBase<InternetShopContext>, IOrdersContext
    {
        public OrdersContext(InternetShopContext dbContext) : base(dbContext)
        {
        }

        public IEntitySet<Order> Orders => base.GetEntitySet<Order>();

    }
}
