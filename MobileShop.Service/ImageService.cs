using MobileShop.Entity.DTOs.ImageDTO;
using MobileShop.Entity.Models;

namespace MobileShop.Service
{
    public interface IImageService
    {
        List<Image> GetAllImage();
        Image? GetImageById(int id);
        CreateImageResponse AddImage(CreateImageRequest image);
        UpdateImageResponse UpdateImage(UpdateImageRequest image);
        bool UpdateDeleteStatusImage(int id);
        Image? GetImageByLink(string link);
    }


    public class ImageService : IImageService
    {
        private readonly FstoreContext _context;

        public ImageService(FstoreContext context)
        {
            _context = context;
        }

        public List<Image> GetAllImage()
        {
            try
            {
                var images = _context.Images.Where(c => c.IsDeleted == false)
                     .ToList();
                return images;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Image? GetImageById(int id)
        {
            try
            {
                var image = _context.Images.FirstOrDefault(i => i.ImageId == id && i.IsDeleted == false);
                return image ?? null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Image? GetImageByLink(string link)
        {
            try
            {
                var image = _context.Images.FirstOrDefault(i => i.ImageLink != null && i.ImageLink.ToLower().Equals(link.ToLower()) && i.IsDeleted == false);
                return image;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CreateImageResponse AddImage(CreateImageRequest image)
        {
            try
            {
                var requestImage = new Image
                {
                    ImageLink = image.ImageLink,
                    CreateDate = image.CreateDate,
                    IsDeleted = image.IsDeleted

                };
                _context.Images.Add(requestImage);
                _context.SaveChanges();
                return new CreateImageResponse { IsSuccess = true, Message = "Add image complete" };
            }
            catch (Exception e)
            {
                return new CreateImageResponse { IsSuccess = false, Message = $"Add image failed {e.Message}" };

            }
        }

        public UpdateImageResponse UpdateImage(UpdateImageRequest image)
        {
            try
            {
                var requestImage = new Image
                {
                    ImageId = image.ImageId,
                    ImageLink = image.ImageLink,
                    CreateDate = image.CreateDate,
                    IsDeleted = image.IsDeleted

                };
                _context.Images.Update(requestImage);
                _context.SaveChanges();
                return new UpdateImageResponse { IsSuccess = true, Message = "Update image complete" };
            }
            catch (Exception e)
            {
                return new UpdateImageResponse { IsSuccess = false, Message = $"Update image failed {e.Message}" };

            }
        }

        public bool UpdateDeleteStatusImage(int id)
        {
            try
            {
                var image = GetImageById(id);
                if (image == null)
                {
                    return false;
                }
                image.IsDeleted = true;
                _context.Images.Update(image);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
