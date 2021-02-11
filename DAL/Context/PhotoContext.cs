using System;
using System.Collections.Generic;
using System.Text;
using BSUIR.DAL.Interfaces;
using BSUIR.DAL.Interfaces.Context;
using BSUIR.DAL.Interfaces.Models;

namespace BSUIR.DAL.Context
{
    public class PhotoContext : AppDomainContextBase<InternetShopContext>, IPhotoContext
    {
        public PhotoContext(InternetShopContext dbContext) : base(dbContext)
        {
        }

        public IEntitySet<Photo> Photos => base.GetEntitySet<Photo>();
    }
}
