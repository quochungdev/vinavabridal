using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VinavaFashionProject.Api.Data;
using VinavaFashionProject.Api.DTO;
using VinavaFashionProject.Api.Models;

namespace VinavaFashionProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DbvinavaFashionContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(DbvinavaFashionContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                //Description = p.Description,
                Price = p.Price,
                SaleDiscount = p.SaleDiscount,
                //SizeImage = p.SizeImage,
                Idstring = p.Idstring,
                ImageUrl = p.ImageUrl,
                IsNew = p.IsNew,
                //Quantity = p.Quantity,
                //StorageInstructions = p.StorageInstructions,
                Category = new CategoryDTO
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name,
                    IsAccessory = p.Category.IsAccessory
                },
            }).ToList();

            return Ok(productDTOs);
        }

        // GET: api/products/initial
        [HttpGet("initial")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetInitialProducts()
        {
            var initialProducts = await _context.Products.Take(20).ToListAsync(); // Lấy 10 sản phẩm ban đầu
            var productDTOs = initialProducts.Select(p => new ProductDTO
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                //Description = p.Description,
                Price = p.Price,
                SaleDiscount = p.SaleDiscount,
                //SizeImage = p.SizeImage,
                Idstring = p.Idstring,
                ImageUrl = p.ImageUrl,
                IsNew = p.IsNew,
                //Quantity = p.Quantity,
                //StorageInstructions = p.StorageInstructions,
            }).ToList();
            return Ok(productDTOs);
        }

        //// GET: api/products/more/{skip}/{take}
        //[HttpGet("more/{skip}/{take}")]
        //public async Task<ActionResult<IEnumerable<ProductDTO>>> GetMoreProducts(int skip, int take)
        //{
        //    var moreProducts = await _context.Products.Skip(skip).Take(take).ToListAsync(); // Lấy các sản phẩm tiếp theo từ vị trí skip
        //    var productDTOs = moreProducts.Select(p => new ProductDTO
        //    {
        //        Id = p.Id,
        //        CategoryId = p.CategoryId,
        //        Name = p.Name,
        //        //Description = p.Description,
        //        Price = p.Price,
        //        SaleDiscount = p.SaleDiscount,
        //        //SizeImage = p.SizeImage,
        //        Idstring = p.Idstring,
        //        ImageUrl = p.ImageUrl,
        //        IsNew = p.IsNew,
        //        //Quantity = p.Quantity,
        //        //StorageInstructions = p.StorageInstructions,

        //    }).ToList();
        //    return Ok(productDTOs);
        //}
        [HttpGet("morefiltered")]
        public async Task<IEnumerable<ProductDTO>> GetMoreFilteredProducts(int skip, int take, int categoryId = -1, decimal saleDiscount = -1, string keyword = null, string orderBy = null)
        {
            var query = _context.Products.AsQueryable();

            if (categoryId == -1 && saleDiscount == -1 && string.IsNullOrEmpty(orderBy) && string.IsNullOrEmpty(keyword))
            {
                query = query.Skip(skip).Take(take);
            }
            else
            {
                if (!string.IsNullOrEmpty(orderBy))
                {
                    switch (orderBy.ToLower())
                    {
                        case "priceasc":
                            query = query.OrderBy(p => p.Price);
                            break;
                        case "pricedesc":
                            query = query.OrderByDescending(p => p.Price);
                            break;
                        default:
                            break;
                    }
                }

                if (categoryId != -1)
                {
                    query = query.Where(p => p.CategoryId == categoryId);
                }

                if (saleDiscount != -1)
                {
                    query = query.Where(p => p.SaleDiscount == saleDiscount);
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(p => p.Name.Contains(keyword));
                }

                // Skip và Take
                query = query.Skip(skip).Take(take);
            }

            // Thực hiện truy vấn để lấy sản phẩm phù hợp
            var filteredProducts = await query.Select(p => new ProductDTO
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                Price = p.Price,
                SaleDiscount = p.SaleDiscount,
                Idstring = p.Idstring,
                ImageUrl = p.ImageUrl,
                IsNew = p.IsNew,
            }).ToListAsync();
            return filteredProducts;
        }


        [HttpGet("IsNew")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsIsNew()
        {
            var products = await _context.Products
                .Where(p => p.IsNew == true)
                .ToListAsync();

            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                //Description = p.Description,
                Price = p.Price,
                Idstring = p.Idstring,
                ImageUrl = p.ImageUrl,
                IsNew = p.IsNew,
                //Quantity = p.Quantity,
                //StorageInstructions = p.StorageInstructions,
            }).ToList();

            return Ok(productDTOs);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailDTO>> GetProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductAttributes)
                    .ThenInclude(pa => pa.Attribute)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDTO = new ProductDetailDTO
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SizeImage = product.SizeImage,
                Idstring = product.Idstring,
                Quantity = product.Quantity,
                StorageInstructions = product.StorageInstructions,

                Category = new CategoryDTO
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name,
                    IsAccessory = product.Category.IsAccessory
                },
                ProductAttributes = product.ProductAttributes.Select(pa => new ProductAttributeDTO
                {
                    Id = pa.Id,
                    ProductId = pa.ProductId,
                    AttributeId = pa.AttributeId,
                    Attribute = new AttributeDTO
                    {
                        Id = pa.Attribute.Id,
                        AttributeName = pa.Attribute.AttributeName,
                        AttributeValue = pa.Attribute.AttributeValue
                    }
                }).ToList(),
                //ProductImages = product.ProductImages.Select(pi => new ProductImageDTO
                //{
                //    Id = pi.Id,
                //    ProductId = pi.ProductId,
                //    ImageUrl = pi.ImageUrl
                //}).ToList()
            };

            return Ok(productDTO);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'DbvinavaFashionContext.Products'  is null.");
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: api/Products/GetAndSaveProductImages
        [HttpGet("GetAndSaveProductImages")]
        public async Task<IActionResult> GetAndSaveProductImages()
        {
            try
            {
                var productIdsString = await _context.Products.Select(p => p.Idstring).ToListAsync();
                foreach (var productId in productIdsString)
                {
                    //string folderPath = $"~/Images/{productId}/image_product_main";
                    string folderPath = Path.Combine(_env.ContentRootPath, "Images", productId);

                    if (Directory.Exists(folderPath))
                    {
                        string[] filePaths = Directory.GetFiles(folderPath, "image_product_main.*");
                        foreach (string filePath in filePaths)
                        {
                            byte[] fileData = await System.IO.File.ReadAllBytesAsync(filePath);
                            var product = await _context.Products.FirstOrDefaultAsync(p => p.Idstring == productId);
                            if (product != null)
                            {
                                string ImageBase64 = Convert.ToBase64String(fileData);
                                product.ImageUrl = ImageBase64;
                            }
                        }
                    }
                    else
                        return NotFound();
                }
                await _context.SaveChangesAsync();
                return Ok("Luu hình ảnh vào cơ sở dữ liệu thành công.");
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine(innerException.Message);
                    innerException = innerException.InnerException;
                }

                // Trả về thông báo lỗi
                return StatusCode(500, "Lỗi khi lưu thay đổi vào cơ sở dữ liệu");
            }
        }

        // GET: api/Products/GetAndSaveSizeImages
        [HttpGet("GetAndSaveSizeImages")]
        public async Task<IActionResult> GetAndSaveSizeImages()
        {
            try
            {
                var productIdsString = await _context.Products.Select(p => p.Idstring).ToListAsync();
                foreach (var productId in productIdsString)
                {
                    //string folderPath = $"~/Images/{productId}/image_product_main";
                    string folderPath = Path.Combine(_env.ContentRootPath, "Images", productId);

                    if (Directory.Exists(folderPath))
                    {
                        string[] filePaths = Directory.GetFiles(folderPath, "SizeImage.*");
                        foreach (string filePath in filePaths)
                        {
                            byte[] fileData = await System.IO.File.ReadAllBytesAsync(filePath);
                            var product = await _context.Products.FirstOrDefaultAsync(p => p.Idstring == productId);
                            if (product != null)
                            {
                                string ImageBase64 = Convert.ToBase64String(fileData);
                                product.SizeImage = ImageBase64;
                            }
                        }
                    }
                    else
                        return NotFound();
                }
                await _context.SaveChangesAsync();
                return Ok("Luu hình ảnh vào cơ sở dữ liệu thành công.");
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine(innerException.Message);
                    innerException = innerException.InnerException;
                }

                // Trả về thông báo lỗi
                return StatusCode(500, "Lỗi khi lưu thay đổi vào cơ sở dữ liệu");
            }
        }

        // GET: api/Products/GetAndSaveProductImagesChild
        [HttpGet("GetAndSaveProductImagesChild")]
        public async Task<IActionResult> GetAndSaveProductImagesChild()
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Product_Image");
                await transaction.CommitAsync();

                var productIds = await _context.Products.Select(p => new { p.Id, p.Idstring }).ToListAsync();
                foreach (var product in productIds)
                {
                    string folderPath = Path.Combine(_env.ContentRootPath, "Images", product.Idstring);

                    if (Directory.Exists(folderPath))
                    {
                        string[] filePaths = Directory.GetFiles(folderPath, "image_product_child_*");
                        foreach (string filePath in filePaths)
                        {
                            byte[] fileData = await System.IO.File.ReadAllBytesAsync(filePath);
                            string imageBase64 = Convert.ToBase64String(fileData);

                            var productImage = new ProductImage
                            {
                                ProductId = product.Id,
                                ImageUrl = imageBase64,
                            };

                            _context.ProductImages.Add(productImage);



                            //var existingProductImage = await _context.ProductImages
                            //    .FirstOrDefaultAsync(pi => pi.ProductId == product.Id);

                            //if (existingProductImage != null)
                            //{
                            //    existingProductImage.ImageUrl = imageBase64;
                            //}
                            //else
                            //{
                            //    var productImage = new ProductImage
                            //    {
                            //        ProductId = product.Id,
                            //        ImageUrl = imageBase64,
                            //    };

                            //    _context.ProductImages.Add(productImage);
                            //}
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return Ok("Đã xóa dữ liệu cũ và lưu hình ảnh con vào cơ sở dữ liệu thành công.");
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Lỗi khi lưu thay đổi vào cơ sở dữ liệu. Xem chi tiết trong inner exception.");
            }
        }

        [HttpGet("/api/products/{productId}/image")]
        public async Task<IActionResult> GetProductImageAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                string imageBase64 = product.ImageUrl;

                return Content(imageBase64, "text/plain");
            }
            return NotFound();
        }

        // GET: api/Products/{productId}/ProductDetailImages
        [HttpGet("{productId}/ProductDetailImages")]
        public async Task<ActionResult<IEnumerable<ProductImageDTO>>> GetProductImages(int productId)
        {
            var productImages = await _context.ProductImages
                            .Where(pi => pi.ProductId == productId)
                            .ToListAsync();

            if (productImages == null || !productImages.Any())
            {
                return NotFound();
            }

            var productImagesDTO = productImages.Select(pi => new ProductImageDTO
            {

                Id = pi.Id,
                ProductId = pi.ProductId,
                ImageUrl = pi.ImageUrl
            }).ToList();

            return productImagesDTO;

        }

        [HttpGet("category/{categoryId}/products")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(int categoryId)
        {
            var category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
            {
                return NotFound("Không tìm thấy Category");
            }

            var products = category.Products.Take(20).ToList();
            if (products == null || products.Count == 0)
            {
                return NotFound("Không tìm thấy sản phẩm trong category");
            }

            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                //Description = p.Description,
                Price = p.Price,
                //Quantity = p.Quantity,
                ImageUrl = p.ImageUrl,
                //StorageInstructions = p.StorageInstructions,
                //SizeImage = p.SizeImage,
                IsNew = p.IsNew,
                SaleDiscount = p.SaleDiscount,
                Idstring = p.Idstring
            }).ToList();
            return Ok(productDTOs);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchProducts(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return BadRequest("Keyword không hợp lệ");
            }

            var products = await _context.Products.Where(p => p.Name.Contains(keyword)).Take(20).ToListAsync();
            if (products == null || products.Count == 0)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }

            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                //Description = p.Description,
                Price = p.Price,
                SaleDiscount = p.SaleDiscount,
                //SizeImage = p.SizeImage,
                Idstring = p.Idstring,
                ImageUrl = p.ImageUrl,
                IsNew = p.IsNew,
                //Quantity = p.Quantity,
                //StorageInstructions = p.StorageInstructions,
                Category = new CategoryDTO
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name,
                    IsAccessory = p.Category.IsAccessory
                },
            }).ToList();

            return Ok(productDTOs);
        }

        //Sắp xếp giá sản phẩm tăng dần
        [HttpGet("PriceAsc")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsOrderedByPriceAsc()
        {
            var products = await _context.Products.OrderBy(p => p.Price).Take(20).ToListAsync();
            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                //Description = p.Description,
                Price = p.Price,
                SaleDiscount = p.SaleDiscount,
                //SizeImage = p.SizeImage,
                Idstring = p.Idstring,
                ImageUrl = p.ImageUrl,
                IsNew = p.IsNew,
                //Quantity = p.Quantity,
                //StorageInstructions = p.StorageInstructions,
                Category = new CategoryDTO
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name,
                    IsAccessory = p.Category.IsAccessory
                },
            }).ToList();

            return Ok(productDTOs);
        }

        //Sắp xếp giá sản phẩm giảm dần
        [HttpGet("PriceDesc")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsOrderedByPriceDesc()
        {
            var products = await _context.Products.OrderByDescending(p => p.Price).Take(20).ToListAsync();
            var productDTOs = products.Select(p => new ProductDTO
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Name = p.Name,
                //Description = p.Description,
                Price = p.Price,
                SaleDiscount = p.SaleDiscount,
                //SizeImage = p.SizeImage,
                Idstring = p.Idstring,
                ImageUrl = p.ImageUrl,
                IsNew = p.IsNew,
                //Quantity = p.Quantity,
                //StorageInstructions = p.StorageInstructions,
                Category = new CategoryDTO
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name,
                    IsAccessory = p.Category.IsAccessory
                },
            }).ToList();

            return Ok(productDTOs);
        }
    }
}