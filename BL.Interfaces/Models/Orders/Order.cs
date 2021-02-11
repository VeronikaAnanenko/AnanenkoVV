using System;

namespace BSUIR.BL.Interfaces.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public decimal Amount { get; set; }
        public string CustomerId { get; set; }
        public int? DeliveryAddressId { get; set; }
        public int? AddressId { get; set; }
    }
}
