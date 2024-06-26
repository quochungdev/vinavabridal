﻿using System;
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
    public class AddressesController : ControllerBase
    {
        private readonly DbvinavaFashionContext _context;

        public AddressesController(DbvinavaFashionContext context)
        {
            _context = context;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
        {
            if (_context.Addresses == null)
            {
                return NotFound();
            }
            return await _context.Addresses.ToListAsync();
        }

        [HttpGet("GetAddressesByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<AddressDTO>>> GetAddressesByUserId(int userId)
        {
            {
                var addressesDTO = await _context.Addresses
                    .Where(a => a.UserId == userId)
                    .Select(au => new AddressDTO
                    {
                        Id = au.Id,
                        FullName = au.FullName,
                        City = au.City,
                        DetailAddress = au.DetailAddress,
                        PhoneNumber = au.PhoneNumber,
                        UserId = userId,
                        Ward = au.Ward,

                    }).ToListAsync();

                if (addressesDTO == null || !addressesDTO.Any())
                {
                    return NotFound();
                }

                return addressesDTO;
            }
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            if (_context.Addresses == null)
            {
                return NotFound();
            }
            var address = await _context.Addresses.FindAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }

        // PUT: api/Addresses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, Address address)
        {
            if (id != address.Id)
            {
                return BadRequest();
            }

            _context.Entry(address).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
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

        // POST: api/Addresses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            if (_context.Addresses == null)
            {
                return Problem("Entity set 'DbvinavaFashionContext.Addresses'  is null.");
            }
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAddress", new { id = address.Id }, address);
        }

        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (_context.Addresses == null)
            {
                return NotFound();
            }
            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AddressExists(int id)
        {
            return (_context.Addresses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
