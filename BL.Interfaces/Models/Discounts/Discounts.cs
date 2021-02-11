namespace BSUIR.BL.Interfaces.Models.Discounts
{
    public class Discounts
    {
        public int Id { get; set; }
        public decimal From { get; set; }
        public decimal To { get; set; }
        public int? Amount { get; set; }
    }
}
