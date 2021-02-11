using System;
using System.Collections.Generic;

namespace BSUIR.DAL.Interfaces.Models
{
    public partial class Address
    {
        public Address()
        {
            Order = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int House { get; set; }
        public int PostIndex { get; set; }
        public string Region { get; set; }
        public int? Flat { get; set; }
        public string CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
