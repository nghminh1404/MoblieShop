using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileShop.Entity.DTOs.AccountDTO;
using MobileShop.Entity.DTOs.CategoryDTO;
using MobileShop.Service;

namespace MobileShop.API.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpPost("add-category")]
        public IActionResult AddCategory([FromBody] CreateCategoryRequest category)
        {
            var result = _categoryService.AddCategory(category);
            return Ok(result);
        }

        [HttpGet("get-all-category")]
        public IActionResult GetAllCategory()
        {
            var categories = _categoryService.GetAllCategory();
            return categories.Count == 0 ? Ok("Don't have category") : Ok(categories);
        }

        [HttpGet("get-category-id/{id:int}")]
        public IActionResult GetAccountById(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound("Category does not exist");
            }

            return Ok(category);
        }

        [HttpGet("get-category-name/{name}")]
        public IActionResult GetAccountByName(string name)
        {
            var category = _categoryService.GetCategoryByName(name);
            if (category == null)
            {
                return NotFound("Category does not exist");
            }

            return Ok(category);
        }

        [HttpPut("put-category")]
        public IActionResult UpdateCategory(UpdateCategoryRequest category)
        {
            var result = _categoryService.UpdateCategory(category);
            return Ok(result);
        }

        [HttpDelete("delete-category/{id:int}")]
        public IActionResult DeleteAccount(int id)
        {
            var result = _categoryService.UpdateDeleteStatusCategory(id);
            if (result == false)
            {
                return StatusCode(500);
            }

            return Ok("Delete category complete");
        }
    }
}