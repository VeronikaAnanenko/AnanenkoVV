using System;
using System.Collections.Generic;

namespace BSUIR.DAL.Interfaces.Models
{
    public partial class Discounts
    {
        public int Id { get; set; }
        public decimal From { get; set; }
        public decimal To { get; set; }
        public int? Amount { get; set; }
    }
}
