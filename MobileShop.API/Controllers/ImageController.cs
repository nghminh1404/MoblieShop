using Microsoft.AspNetCore.Mvc;
using MobileShop.Entity.DTOs.ImageDTO;
using MobileShop.Service;

namespace MobileShop.API.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("add-image")]
        public IActionResult AddCategory([FromBody] CreateImageRequest image)
        {
            var result = _imageService.AddImage(image);
            return Ok(result);
        }

        [HttpGet("get-all-image")]
        public IActionResult GetAllImage()
        {
            var images = _imageService.GetAllImage();
            return images.Count == 0 ? Ok("Don't have image") : Ok(images);
        }

        [HttpGet("get-image-link/{link}")]
        public IActionResult GetImageByLink(string link)
        {
            var image = _imageService.GetImageByLink(link);
            if (image == null)
            {
                return NotFound("Image does not exist");
            }

            return Ok(image);
        }

        [HttpGet("get-image-id/{id:int}")]
        public IActionResult GetImageById(int id)
        {
            var image = _imageService.GetImageById(id);
            if (image == null)
            {
                return NotFound("Image does not exist");
            }

            return Ok(image);
        }

        [HttpPut("put-image")]
        public IActionResult UpdateImage(UpdateImageRequest image)
        {
            var result = _imageService.UpdateImage(image);
            return Ok(result);
        }

        [HttpDelete("delete-image/{id:int}")]
        public IActionResult DeleteImage(int id)
        {
            var result = _imageService.UpdateDeleteStatusImage(id);
            if (result == false)
            {
                return StatusCode(500);
            }

            return Ok("Delete image complete");
        }
    }
}