namespace MobileShop.Entity.DTOs.ProductDTO
{
    public class UpdateProductRequest
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public double Price { get; set; }

        public int Quantity { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public int ImageId { get; set; }

        public DateTime? CreateDate { get; set; }

        public int CreateBy { get; set; }

        public bool? IsDeleted { get; set; }

        
    }
}
