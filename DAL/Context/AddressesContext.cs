using BSUIR.DAL.Interfaces;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Models;

namespace BSUIR.DAL.Context
{
    public class AddressesContext : AppDomainContextBase<InternetShopContext>, IAddressesContext
    {
        public AddressesContext(InternetShopContext dbContext) : base(dbContext)
        {
        }

        public IEntitySet<Address> Addresses => base.GetEntitySet<Address>();

    }
}
