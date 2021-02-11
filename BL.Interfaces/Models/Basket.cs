using System;
using System.Collections.Generic;
using System.Text;
using BSUIR.BL.Interfaces.Models.Items;

namespace BSUIR.BL.Interfaces.Models
{
    public class Basket
    {
        public List<Item> Items { get; set; }
        public decimal Price { get; set; }
        public int AddressId { get; set; }
        public bool IsDelivery { get; set; }
        public Basket()
        {
            Items = new List<Item>();
        }
    }
}
