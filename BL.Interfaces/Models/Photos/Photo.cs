using BSUIR.BL.Interfaces.Models.Items;

namespace BSUIR.BL.Interfaces.Models.Photos
{
    public class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ItemId { get; set; }
        public string Link { get; set; }

    }
}
