using System;
using System.Collections.Generic;

namespace BSUIR.DAL.Interfaces.Models
{
    public partial class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemId { get; set; }
        public string Link { get; set; }

        public virtual Item Item { get; set; }
    }
}
