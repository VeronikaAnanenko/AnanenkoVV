using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BSUIR.BL.Interfaces.Models
{
    public class Marker
    {
        [JsonProperty(PropertyName = "lat")]
        public double Lat { get; set; }
        [JsonProperty(PropertyName = "lng")]
        public double Lng { get; set; }
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "addressId")]
        public int AddressId { get; set; }
    }

    public class Coordinates
    {
        [JsonProperty(PropertyName = "lat")]
        public double Lat { get; set; }
        [JsonProperty(PropertyName = "lng")]
        public double Lng { get; set; }
    }
}
