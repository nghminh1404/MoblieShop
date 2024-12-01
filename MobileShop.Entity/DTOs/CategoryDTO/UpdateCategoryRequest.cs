namespace MobileShop.Entity.DTOs.CategoryDTO
{
    public class UpdateCategoryRequest
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public bool? IsDeleted { get; set; }
    }
}
