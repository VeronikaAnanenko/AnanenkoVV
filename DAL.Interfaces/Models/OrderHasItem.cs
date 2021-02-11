using System;
using System.Collections.Generic;

namespace BSUIR.DAL.Interfaces.Models
{
    public partial class OrderHasItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ItemId { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }
}
