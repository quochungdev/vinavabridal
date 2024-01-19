using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VinavaFashionProject.Api.Data;
using VinavaFashionProject.Api.Models;

namespace VinavaFashionProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalDbsController : ControllerBase
    {
        private readonly DbvinavaFashionContext _context;

        public PayPalDbsController(DbvinavaFashionContext context)
        {
            _context = context;
        }

        // GET: api/PayPalDbs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PayPalDb>>> GetPayPalDbs()
        {
            if (_context.PayPalDbs == null)
            {
                return NotFound();
            }
            return await _context.PayPalDbs.ToListAsync();
        }

        // GET: api/PayPalDbs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PayPalDb>> GetPayPalDb(int id)
        {
            if (_context.PayPalDbs == null)
            {
                return NotFound();
            }
            var payPalDb = await _context.PayPalDbs.FindAsync(id);

            if (payPalDb == null)
            {
                return NotFound();
            }

            return payPalDb;
        }

        [HttpGet("BankAccount")]
        public async Task<ActionResult<BankAccount>> GetBankAccount()
        {
            try
            {
                var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync();

                if (bankAccount == null)
                {
                    return NotFound();
                }

                return Ok(bankAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("data")]
        public async Task<ActionResult<PayPalDb>> GetPayPalData()
        {
            try
            {
                var paypalData = await _context.PayPalDbs.FirstOrDefaultAsync();

                if (paypalData == null)
                {
                    return NotFound();
                }

                return Ok(paypalData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/PayPalDbs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayPalDb(int id, PayPalDb payPalDb)
        {
            if (id != payPalDb.Id)
            {
                return BadRequest();
            }

            _context.Entry(payPalDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayPalDbExists(id))
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

        // POST: api/PayPalDbs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PayPalDb>> PostPayPalDb(PayPalDb payPalDb)
        {
            if (_context.PayPalDbs == null)
            {
                return Problem("Entity set 'DbvinavaFashionContext.PayPalDbs'  is null.");
            }
            _context.PayPalDbs.Add(payPalDb);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayPalDb", new { id = payPalDb.Id }, payPalDb);
        }

        // DELETE: api/PayPalDbs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayPalDb(int id)
        {
            if (_context.PayPalDbs == null)
            {
                return NotFound();
            }
            var payPalDb = await _context.PayPalDbs.FindAsync(id);
            if (payPalDb == null)
            {
                return NotFound();
            }

            _context.PayPalDbs.Remove(payPalDb);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PayPalDbExists(int id)
        {
            return (_context.PayPalDbs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
