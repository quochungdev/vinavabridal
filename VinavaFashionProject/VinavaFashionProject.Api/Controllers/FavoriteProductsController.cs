using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VinavaFashionProject.Api.Data;
using VinavaFashionProject.Api.DTO;
using VinavaFashionProject.Api.Models;

namespace VinavaFashionProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteProductsController : ControllerBase
    {
        private readonly DbvinavaFashionContext _context;

        public FavoriteProductsController(DbvinavaFashionContext context)
        {
            _context = context;
        }

        // GET: api/FavoriteProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavoriteProduct>>> GetFavoriteProducts()
        {
            if (_context.FavoriteProducts == null)
            {
                return NotFound();
            }
            return await _context.FavoriteProducts.ToListAsync();
        }

        // GET: api/FavoriteProducts/user/{userId}/{productId}
        [HttpGet("user/{userId}/{productId}")]
        public async Task<ActionResult<FavoriteProductDTO>> GetFavoriteProductByUserIdAndProductId(int userId, int productId)
        {
            try
            {
                var favoriteProduct = await _context.FavoriteProducts
                    .FirstOrDefaultAsync(fp => fp.UserId == userId && fp.ProductId == productId);

                if (favoriteProduct == null)
                {
                    return NotFound();
                }

                var favoriteProductDTO = new FavoriteProductDTO
                {
                    Id = favoriteProduct.Id,
                    UserId = favoriteProduct.UserId,
                    ProductId = favoriteProduct.ProductId,
                    FavoriteDate = favoriteProduct.FavoriteDate
                };

                return favoriteProductDTO;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/FavoriteProducts/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<FavoriteProductDTO>>> GetFavoriteProductsByUserId(int userId)
        {
            try
            {
                var favoriteProducts = await _context.FavoriteProducts
                    .Where(fp => fp.UserId == userId)
                    .ToListAsync();

                if (favoriteProducts == null || favoriteProducts.Count == 0)
                {
                    return NotFound();
                }

                var favoriteProductDTOs = favoriteProducts.Select(fp => new FavoriteProductDTO
                {
                    Id = fp.Id,
                    UserId = fp.UserId,
                    ProductId = fp.ProductId,
                    FavoriteDate = fp.FavoriteDate,
                    Product = new FavouriteProduct_ProductDTO
                    {
                        Id = fp.Product.Id,
                        Name = fp.Product.Name,
                        Price = fp.Product.Price,
                        ImageUrl = fp.Product.ImageUrl
                    }
                }).ToList();

                return favoriteProductDTOs;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/FavoriteProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FavoriteProduct>> GetFavoriteProduct(int id)
        {
            if (_context.FavoriteProducts == null)
            {
                return NotFound();
            }
            var favoriteProduct = await _context.FavoriteProducts.FindAsync(id);

            if (favoriteProduct == null)
            {
                return NotFound();
            }

            return favoriteProduct;
        }

        // PUT: api/FavoriteProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFavoriteProduct(int id, FavoriteProduct favoriteProduct)
        {
            if (id != favoriteProduct.Id)
            {
                return BadRequest();
            }

            _context.Entry(favoriteProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoriteProductExists(id))
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

        // POST: api/FavoriteProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FavoriteProduct>> PostFavoriteProduct(FavoriteProduct favoriteProduct)
        {
            if (_context.FavoriteProducts == null)
            {
                return Problem("Entity set 'DbvinavaFashionContext.FavoriteProducts'  is null.");
            }
            _context.FavoriteProducts.Add(favoriteProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavoriteProduct", new { id = favoriteProduct.Id }, favoriteProduct);
        }

        [HttpDelete("user/{userId}/product/{productId}")]
        public async Task<IActionResult> DeleteFavoriteProduct(int userId, int productId)
        {
            try
            {
                var favoriteProduct = await _context.FavoriteProducts
                    .Where(fp => fp.UserId == userId && fp.ProductId == productId)
                    .FirstOrDefaultAsync();

                if (favoriteProduct == null)
                {
                    return NotFound();
                }

                _context.FavoriteProducts.Remove(favoriteProduct);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private bool FavoriteProductExists(int id)
        {
            return (_context.FavoriteProducts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
