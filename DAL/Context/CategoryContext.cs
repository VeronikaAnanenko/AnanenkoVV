using System;
using System.Collections.Generic;
using System.Text;
using BSUIR.DAL.Interfaces;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Models;

namespace BSUIR.DAL.Context
{
    public class CategoryContext : AppDomainContextBase<InternetShopContext>, ICategoryContext
    {
        public CategoryContext(InternetShopContext dbContext) : base(dbContext)
        {
        }

        public IEntitySet<Category> Categories => base.GetEntitySet<Category>();
    }
}
