namespace FinalProject.Models
{
    public class ProductDiscount : BaseEntity
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int DiscountId { get; set; }
        public Discount Discount { get; set; }

    }
}
