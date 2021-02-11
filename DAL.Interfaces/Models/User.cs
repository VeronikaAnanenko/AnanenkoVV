using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace BSUIR.DAL.Interfaces.Models
{
    public class User :IdentityUser
    {
        public virtual Customer Customer { get; set; }
    }
}
