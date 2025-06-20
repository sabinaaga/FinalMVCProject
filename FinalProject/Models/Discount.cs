namespace FinalProject.Models
{
    public class Discount:BaseEntity
    {
        public decimal Percentage { get; set; }
        public ICollection<ProductDiscount> ProductDiscounts { get; set; }
    }
}
