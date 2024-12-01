using MobileShop.Entity.DTOs.CategoryDTO;
using MobileShop.Entity.Models;

namespace MobileShop.Service
{
    public interface ICategoryService
    {
        List<Category> GetAllCategory();
        Category? GetCategoryById(int id);
        CreateCategoryResponse AddCategory(CreateCategoryRequest category);
        UpdateCategoryResponse UpdateCategory(UpdateCategoryRequest category);
        bool UpdateDeleteStatusCategory(int id);
        Category? GetCategoryByName(string name);
    }


    public class CategoryService : ICategoryService
    {
        private readonly FstoreContext _context;

        public CategoryService(FstoreContext context)
        {
            _context = context;
        }

        public List<Category> GetAllCategory()
        {
            try
            {
                var categories = _context.Categories.Where(c => c.IsDeleted == false).ToList();
                return categories;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Category? GetCategoryById(int id)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id && c.IsDeleted == false);
                return category ?? null;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Category? GetCategoryByName(string name)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(c => c.CategoryName.ToUpper().Equals(name.ToUpper()) && c.IsDeleted == false);
                return category ?? null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CreateCategoryResponse AddCategory(CreateCategoryRequest category)
        {
            try
            {
                var requestCategory = new Category
                {
                    CategoryName = category.CategoryName,
                    IsDeleted = category.IsDeleted
                };
                _context.Categories.Add(requestCategory);
                _context.SaveChanges();
                return new CreateCategoryResponse { IsSuccess = true, Message = "Add category complete" };
            }
            catch (Exception e)
            {
                return new CreateCategoryResponse { IsSuccess = false, Message = $"Add category failed {e.Message}" };

            }
        }

        public UpdateCategoryResponse UpdateCategory(UpdateCategoryRequest category)
        {
            try
            {
                var requestCategory = new Category
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    IsDeleted = category.IsDeleted
                };
                _context.Categories.Update(requestCategory);
                _context.SaveChanges();
                return new UpdateCategoryResponse { IsSuccess = true, Message = "Update category complete" };
            }
            catch (Exception e)
            {
                return new UpdateCategoryResponse { IsSuccess = false, Message = $"Update category failed {e.Message}" };

            }
        }

        public bool UpdateDeleteStatusCategory(int id)
        {
            try
            {
                var category = GetCategoryById(id);
                if (category == null)
                {
                    return false;
                }
                category.IsDeleted = true;
                _context.Update(category);
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
