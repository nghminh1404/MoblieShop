using Microsoft.AspNetCore.Mvc;
using MobileShop.Entity.DTOs.ProductDTO;
using MobileShop.Service;

namespace MobileShop.API.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("add-product")]
        public IActionResult AddProduct([FromBody] CreateProductRequest product)
        {
            var result = _productService.AddProduct(product);
            return Ok(result);
        }

        [HttpGet("get-all-product")]
        public IActionResult GetAllProduct()
        {
            var products = _productService.GetAllProduct();
            return products.Count == 0 ? Ok("Don't have product") : Ok(products);
        }

        [HttpGet("get-product-id/{id:int}")]
        public IActionResult GetAccountById(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound("Product does not exist");
            }

            return Ok(product);
        }

        [HttpGet("get-product-category/{id:int}")]
        public IActionResult GetProductsByCategoryId(int id)
        {
            var product = _productService.GetProductsByCategoryId(id);
            return Ok(product);
        }

        [HttpGet("get-product-keyword&category/{keyword}&{cid:int}")]
        public IActionResult GetProductsByKeywordAndCategoryId(string keyword, int cid)
        {
            var product = _productService.GetProductsByKeywordAndCategoryId(keyword, cid);
            return Ok(product);
        }

        [HttpGet("get-product-keyword/{keyword}")]
        public IActionResult GetProductsByKeyword(string keyword)
        {
            var product = _productService.GetProductsByKeyword(keyword);
            return Ok(product);
        }


        [HttpPut("put-product")]
        public IActionResult UpdateProduct(UpdateProductRequest product)
        {
            var result = _productService.UpdateProduct(product);
            return Ok(result);
        }


        [HttpDelete("delete-product/{id:int}")]
        public IActionResult DeleteProduct(int id)
        {
            var result = _productService.UpdateDeleteStatusProduct(id);
            if (result == false)
            {
                return StatusCode(500);
            }

            return Ok("Delete product complete");
        }
    }
}