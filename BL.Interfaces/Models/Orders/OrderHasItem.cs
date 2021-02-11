using System;
using System.Collections.Generic;
using System.Text;

namespace BSUIR.BL.Interfaces.Models.Orders
{
    public class OrderHasItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }

    }
}
