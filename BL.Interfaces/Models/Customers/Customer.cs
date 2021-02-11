using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BSUIR.BL.Interfaces.Models.Orders;
using BSUIR.BL.Interfaces.Models.Users;

namespace BSUIR.BL.Interfaces.Models.Customers
{
    public class Customer
    {
        public string Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Secondname { get; set; }
        public DateTime Birthdate { get; set; }
        public string MobileNumber { get; set; }
        public string Sex { get; set; }
        public int? Discount { get; set; }
        public string UserId { get; set; }
    }
}
