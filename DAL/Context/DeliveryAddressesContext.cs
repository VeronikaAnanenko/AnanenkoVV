using BSUIR.DAL.Interfaces;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Models;

namespace BSUIR.DAL.Context
{
    public class DeliveryAddressesContext : AppDomainContextBase<InternetShopContext>, IDeliveryAddressesContext
    {
        public DeliveryAddressesContext(InternetShopContext dbContext) : base(dbContext)
        {
        }

        public IEntitySet<DeliveryAddress> DeliveryAddresses => base.GetEntitySet<DeliveryAddress>();
    }
}
