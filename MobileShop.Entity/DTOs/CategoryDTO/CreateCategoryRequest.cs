namespace MobileShop.Entity.DTOs.CategoryDTO
{
    public class CreateCategoryRequest
    {
        public string CategoryName { get; set; } = null!;

        public bool? IsDeleted { get; set; }
    }
}
