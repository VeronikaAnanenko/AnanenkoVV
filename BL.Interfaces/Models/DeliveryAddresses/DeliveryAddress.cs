using System.Collections.Generic;

namespace BSUIR.BL.Interfaces.Models.DeliveryAddresses
{
    public class DeliveryAddress
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int House { get; set; }
        public int? Flat { get; set; }
        public int PostIndex { get; set; }
        public string WorkingHours { get; set; }
        public string Coordinates { get; set; }

    }
}
