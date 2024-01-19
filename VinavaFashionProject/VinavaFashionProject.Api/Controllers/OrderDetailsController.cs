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
    public class OrderDetailsController : ControllerBase
    {
        private readonly DbvinavaFashionContext _context;

        public OrderDetailsController(DbvinavaFashionContext context)
        {
            _context = context;
        }

        [HttpGet("GetOrderDetailsByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDetailByUserDTO>>> GetOrderDetailsByUserId(int userId)
        {
            var orderDetails = await _context.OrderDetails
                .Where(od => od.UserId == userId)
                .Select(od => new OrderDetailByUserDTO
                {
                    Id = od.Id,
                    UserId = od.UserId,
                    Color = od.Color,
                    Price = od.Price,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    Size = od.Size,
                    Total = od.Total,
                    Product = new ProductDTO
                    {
                        Id = od.Product.Id,
                        Name = od.Product.Name,
                        Price = od.Product.Price,
                        ImageUrl = od.Product.ImageUrl,
                        CategoryId = od.Product.CategoryId,
                        //Description = od.Product.Description,
                        IsNew = od.Product.IsNew,
                        Idstring = od.Product.Idstring,
                        SaleDiscount = od.Product.SaleDiscount,
                        //SizeImage = od.Product.SizeImage,
                        //StorageInstructions = od.Product.StorageInstructions,
                        //Quantity = od.Product.Quantity,
                        ProductAttributes = od.Product.ProductAttributes.Select(pa => new ProductAttributeDTO
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
                    }
                }).ToListAsync();

            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound();
            }

            return orderDetails;
        }


        [HttpGet("GetOrderDetailsCartByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDetailByUserDTO>>> GetOrderDetailsCartByUserId(int userId)
        {
            var orderDetails = await _context.OrderDetails
                .Where(od => od.UserId == userId && od.OrderId == null)
                .Select(od => new OrderDetailByUserDTO
                {
                    Id = od.Id,
                    UserId = od.UserId,
                    Color = od.Color,
                    Price = od.Price,
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    Size = od.Size,
                    Total = od.Total,
                    Product = new ProductDTO
                    {
                        Id = od.Product.Id,
                        Name = od.Product.Name,
                        Price = od.Product.Price,
                        ImageUrl = od.Product.ImageUrl,
                        CategoryId = od.Product.CategoryId,
                        //Description = od.Product.Description,
                        IsNew = od.Product.IsNew,
                        Idstring = od.Product.Idstring,
                        SaleDiscount = od.Product.SaleDiscount,
                        //SizeImage = od.Product.SizeImage,
                        //StorageInstructions = od.Product.StorageInstructions,
                        //Quantity = od.Product.Quantity,
                        ProductAttributes = od.Product.ProductAttributes.Select(pa => new ProductAttributeDTO
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
                    }
                }).ToListAsync();

            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound();
            }

            return orderDetails;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails()
        {
            if (_context.OrderDetails == null)
            {
                return NotFound();
            }
            return await _context.OrderDetails.ToListAsync();
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int id)
        {
            if (_context.OrderDetails == null)
            {
                return NotFound();
            }
            var orderDetail = await _context.OrderDetails.FindAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return orderDetail;
        }

        // PUT: api/OrderDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
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

        [HttpPut("UpdateColor/{id}")]
        public async Task<IActionResult> UpdateColor(int id, string newColor)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            orderDetail.Color = newColor;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
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

        [HttpPut("UpdateSize/{id}")]
        public async Task<IActionResult> UpdateSize(int id, string newSize)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            orderDetail.Size = newSize;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
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

        [HttpPut("UpdateQuantity/{id}")]
        public async Task<IActionResult> UpdateQuantity(int id, int newQuantity)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            orderDetail.Quantity = newQuantity;
            orderDetail.Total = newQuantity * orderDetail.Price;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
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

        // POST: api/OrderDetails
        [HttpPost]
        public async Task<ActionResult<OrderDetail>> PostOrderDetail(OrderDetail orderDetail)
        {
            if (_context.OrderDetails == null)
            {
                return Problem("Entity set 'DbvinavaFashionContext.OrderDetails'  is null.");
            }
            _context.OrderDetails.Add(orderDetail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OrderDetailExists(orderDetail.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrderDetail", new { id = orderDetail.Id }, orderDetail);
        }

        // DELETE: api/OrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            if (_context.OrderDetails == null)
            {
                return NotFound();
            }
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderDetailExists(int id)
        {
            return (_context.OrderDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}