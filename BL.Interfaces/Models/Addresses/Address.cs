using System.ComponentModel.DataAnnotations;

namespace BSUIR.BL.Interfaces.Models.Addresses
{
    public class Address
    {
        public int Id { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public int House { get; set; }
        [Required]
        public int PostIndex { get; set; }
        [Required]
        public string Region { get; set; }
        public int? Flat { get; set; }
        public string CustomerId { get; set; }
    }
}
