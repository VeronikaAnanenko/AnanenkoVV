using BSUIR.DAL.Interfaces;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Models;

namespace BSUIR.DAL.Context
{
    public class CustomersContext : AppDomainContextBase<InternetShopContext>, ICustomersContext
    {
        public CustomersContext(InternetShopContext dbContext) : base(dbContext)
        {
        }

        public IEntitySet<Customer> Customers => base.GetEntitySet<Customer>();
    }
}
