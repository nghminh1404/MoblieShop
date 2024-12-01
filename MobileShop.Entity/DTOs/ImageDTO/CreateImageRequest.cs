namespace MobileShop.Entity.DTOs.ImageDTO
{
    public class CreateImageRequest
    {
        public string? ImageLink { get; set; }

        public DateTime? CreateDate { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
