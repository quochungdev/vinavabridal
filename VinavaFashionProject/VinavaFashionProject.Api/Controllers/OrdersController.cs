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
    public class OrdersController : ControllerBase
    {
        private readonly DbvinavaFashionContext _context;

        public OrdersController(DbvinavaFashionContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders
                              .Select(o => new OrderDTO
                              {
                                  Id = o.Id,
                                  UserId = o.UserId,
                                  AddressId = o.AddressId,
                                  PaymentMethod = o.PaymentMethod,
                                  OrderDate = o.OrderDate,
                                  TotalAmount = o.TotalAmount,
                                  ShippingFee = o.ShippingFee,
                                  Status = o.Status,
                                  Address = new Order_AddressDTO
                                  {
                                      Id = o.Address.Id,
                                      FullName = o.Address.FullName,
                                      City = o.Address.City,
                                      DetailAddress = o.Address.DetailAddress,
                                      PhoneNumber = o.Address.PhoneNumber,
                                      Ward = o.Address.Ward
                                  },
                                  OrderDetails = o.OrderDetails.Select(od => new Order_OrderDetailDTO
                                  {
                                      Id = od.Id,
                                      Product = new Order_OrderDetail_ProductDTO
                                      {
                                          Id = od.Product.Id,
                                          ImageUrl = od.Product.ImageUrl,
                                          Name = od.Product.Name,
                                          Price = od.Product.Price,
                                      },
                                      Color = od.Color,
                                      Size = od.Size,
                                      Quantity = od.Quantity
                                  }).ToList(),
                              })
                              .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // GET: api/Orders/5
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrderByUserId(int userId)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var orders = await _context.Orders
                              .Where(o => o.UserId == userId)
                              .Include(o => o.User)
                              .Select(o => new OrderDTO
                              {
                                  Id = o.Id,
                                  UserId = o.UserId,
                                  AddressId = o.AddressId,
                                  OrderDate = o.OrderDate,
                                  TotalAmount = o.TotalAmount,
                                  ShippingFee = o.ShippingFee,
                                  Status = o.Status,
                                  Address = new Order_AddressDTO
                                  {
                                      Id = o.Address.Id,
                                      FullName = o.Address.FullName,
                                      City = o.Address.City,
                                      DetailAddress = o.Address.DetailAddress,
                                      PhoneNumber = o.Address.PhoneNumber,
                                      Ward = o.Address.Ward
                                  },
                                  OrderDetails = o.OrderDetails.Select(od => new Order_OrderDetailDTO
                                  {
                                      Id = od.Id,
                                      Product = new Order_OrderDetail_ProductDTO
                                      {
                                          Id = od.Product.Id,
                                          ImageUrl = od.Product.ImageUrl,
                                          Name = od.Product.Name,
                                          Price = od.Product.Price,
                                      },
                                      Color = od.Color,
                                      Size = od.Size,
                                      Quantity = od.Quantity
                                  }).ToList(),
                              })
                              .ToListAsync();
            if (orders == null)
            {
                return NotFound();
            }

            return orders;
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<int>> PostOrder(Order order)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'DbvinavaFashionContext.Orders'  is null.");
            }
            _context.Orders.Add(order);
            try
            {
                await _context.SaveChangesAsync();

                int newOrderId = order.Id;
                var orderDetailsToUpdate = _context.OrderDetails.Where(od => od.UserId == order.UserId && od.OrderId == null);
                foreach (var orderDetail in orderDetailsToUpdate)
                {
                    orderDetail.OrderId = newOrderId;
                }

                await _context.SaveChangesAsync();

                return newOrderId;
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("GetExchangeRate")]
        public async Task<ActionResult<decimal>> GetExchangeRate(string currency)
        {
            var exchangeRate = await _context.ExchangeRates
                                        .Where(rate => rate.Currency == currency)
                                        .OrderByDescending(rate => rate.Date)
                                        .Select(rate => rate.Rate)
                                        .FirstOrDefaultAsync();

            if (exchangeRate <= 0)
            {
                return NotFound("Exchange rate not found or invalid for the given currency.");
            }

            return Ok(exchangeRate);
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
