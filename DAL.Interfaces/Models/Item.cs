using System;
using System.Collections.Generic;

namespace BSUIR.DAL.Interfaces.Models
{
    public partial class Item
    {
        public Item()
        {
            Photo = new HashSet<Photo>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public int Power { get; set; }
        public decimal Length { get; set; }
        public decimal Height { get; set; }
        public decimal Width { get; set; }
        public string Color { get; set; }
        public int? Guarantee { get; set; }
        public int? Validity { get; set; }
        public string Country { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        public decimal? Weight { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Photo> Photo { get; set; }
    }
}
