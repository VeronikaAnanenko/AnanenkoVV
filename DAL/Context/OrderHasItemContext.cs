using System;
using System.Collections.Generic;
using System.Text;
using BSUIR.DAL.Interfaces;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Models;

namespace BSUIR.DAL.Context
{
    public class OrderHasItemContext : AppDomainContextBase<InternetShopContext>, IOrderHasItemContext
    {
        public OrderHasItemContext(InternetShopContext dbContext) : base(dbContext)
        {
        }

        public IEntitySet<OrderHasItem> OrderHasItems => base.GetEntitySet<OrderHasItem>();

    }
}
