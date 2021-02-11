using BSUIR.DAL.Interfaces;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Models;

namespace BSUIR.DAL.Context
{
    public class DiscountsContext : AppDomainContextBase<InternetShopContext>, IDiscountsContext
    {
        public DiscountsContext(InternetShopContext dbContext) : base(dbContext)
        {
        }

        public IEntitySet<Discounts> Discounts => base.GetEntitySet<Discounts>();

    }
}
