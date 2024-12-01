using MobileShop.Entity.DTOs.ProductDTO;
using MobileShop.Entity.Models;

namespace MobileShop.Service
{
    public interface IProductService
    {
        List<Product> GetAllProduct();
        Product? GetProductById(int id);
        List<Product> GetProductsByKeyword(string keyword);
        CreateProductResponse AddProduct(CreateProductRequest product);
        UpdateProductResponse UpdateProduct(UpdateProductRequest product);
        bool UpdateDeleteStatusProduct(int id);
        List<Product> GetProductsByCategoryId(int cid);
        List<Product> GetProductsByKeywordAndCategoryId(string keyword, int cid);
    }
    public class ProductService : IProductService
    {
        private readonly FstoreContext _context;

        public ProductService(FstoreContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProduct()
        {
            try
            {
                var products = _context.Products.Where(p => p.IsDeleted == false)
                     .ToList();
                return products;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Product? GetProductById(int id)
        {
            try
            {
                var products = _context.Products.FirstOrDefault(p => p.ProductId == id && p.IsDeleted == false);
                return products ?? null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Product> GetProductsByKeyword(string keyword)
        {
            try
            {
                var products = _context.Products.Where(x => x.ProductName.ToLower().Contains(keyword.ToLower())).ToList();
                return products;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Product> GetProductsByCategoryId(int cid)
        {
            try
            {
                var products = _context.Products.Where(x => x.CategoryId == cid).ToList();
                return products;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Product> GetProductsByKeywordAndCategoryId(string keyword, int cid)
        {
            try
            {
                var products = _context.Products.Where(x => x.ProductName.ToLower().Contains(keyword.ToLower()) 
                                                        && x.CategoryId == cid).ToList();
                return products;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CreateProductResponse AddProduct(CreateProductRequest product)
        {
            try
            {
                var requestProduct = new Product
                {
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    ImageId = product.ImageId,
                    CreateDate = product.CreateDate,
                    CreateBy = product.CreateBy,
                    IsDeleted = product.IsDeleted
                };
                _context.Products.Add(requestProduct);
                _context.SaveChanges();
                return new CreateProductResponse { IsSuccess = true, Message = "Add product complete" };
            }
            catch (Exception e)
            {
                return new CreateProductResponse { IsSuccess = false, Message = $"Add product failed {e.Message}" };

            }
        }

        public UpdateProductResponse UpdateProduct(UpdateProductRequest product)
        {
            try
            {
                var requestProduct = new Product
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    ImageId = product.ImageId,
                    CreateDate = product.CreateDate,
                    CreateBy = product.CreateBy,
                    IsDeleted = product.IsDeleted
                };
                _context.Products.Update(requestProduct);
                _context.SaveChanges();
                return new UpdateProductResponse { IsSuccess = true, Message = "Update product complete" };
            }
            catch (Exception e)
            {
                return new UpdateProductResponse { IsSuccess = false, Message = $"Update product failed {e.Message}" };
            }
        }

        public bool UpdateDeleteStatusProduct(int id)
        {
            try
            {
                var product = GetProductById(id);
                if (product == null)
                {
                    return false;
                }
                product.IsDeleted = true;
                _context.Products.Update(product);
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
