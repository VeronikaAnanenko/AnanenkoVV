using System;
using System.Collections.Generic;

namespace BSUIR.DAL.Interfaces.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Address = new HashSet<Address>();
            Order = new HashSet<Order>();
        }

        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Secondname { get; set; }
        public DateTime Birthdate { get; set; }
        public string MobileNumber { get; set; }
        public string Sex { get; set; }
        public int? Discount { get; set; }

        public virtual User IdNavigation { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
