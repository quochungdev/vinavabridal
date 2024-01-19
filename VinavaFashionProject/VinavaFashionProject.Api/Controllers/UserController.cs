using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using VinavaFashionProject.Api.Data;
using VinavaFashionProject.Api.DTO;
using VinavaFashionProject.Api.Models;
using VinavaFashionProject.Services;
namespace VinavaFashionProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DbvinavaFashionContext _context;
        private readonly IPasswordHasher _passwordHasher = new PasswordHasher();

        public UserController(DbvinavaFashionContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO updatedUserData)
        {
            if (id != updatedUserData.Id)
            {
                return BadRequest();
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Username = updatedUserData.Username ?? existingUser.Username;
            existingUser.FullName = updatedUserData.FullName ?? existingUser.FullName;
            existingUser.Email = updatedUserData.Email ?? existingUser.Email;
            existingUser.FullName = updatedUserData.FullName ?? existingUser.FullName;
            existingUser.Address = updatedUserData.Email ?? existingUser.Email;
            existingUser.DateOfBirth = updatedUserData.DateOfBirth ?? existingUser.DateOfBirth;
            existingUser.Gender = updatedUserData.Gender ?? existingUser.Gender;
            existingUser.CreateDate = updatedUserData.CreateDate ?? existingUser.CreateDate;
            existingUser.CreatedBy = updatedUserData.CreatedBy ?? existingUser.CreatedBy;
            existingUser.ModifiedDate = updatedUserData.ModifiedDate ?? existingUser.DateOfBirth;
            //existingUser.ModifiedBy = updatedUserData.ModifiedBy ?? existingUser.ModifiedBy;
            //existingUser.RowId = updatedUserData.RowId ?? existingUser.RowId;


            //existingUser.Username = updatedUserData.Username;
            //existingUser.FullName = updatedUserData.FullName;
            //existingUser.Email = updatedUserData.Email;
            //existingUser.FullName = updatedUserData.FullName;
            //existingUser.Address = updatedUserData.ModifiedBy;
            //existingUser.DateOfBirth = updatedUserData.DateOfBirth;
            //existingUser.Gender = updatedUserData.Gender;
            //existingUser.CreateDate = updatedUserData.CreateDate;
            //existingUser.CreatedBy = updatedUserData.CreatedBy;
            //existingUser.ModifiedDate = updatedUserData.ModifiedDate;
            //existingUser.ModifiedBy = updatedUserData.ModifiedBy;
            //existingUser.RowId = updatedUserData.RowId;


            _context.Entry(existingUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return NoContent();
            return Ok(new
            {
                updatedUserData.Id,
                updatedUserData.Username,
                updatedUserData.FullName,
                updatedUserData.Email,
                updatedUserData.Address,
                updatedUserData.PhoneNumber,
                updatedUserData.DateOfBirth,
                updatedUserData.Gender
            });
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("create")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DbvinavaFashionContext.Users'  is null.");
            }

            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet("login/{username}/{password}")]
        public async Task<ActionResult<UserDTO>> Login(string username, string password)
        {
            //if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            //{
            //    var user = await _context.Users.Where(x => x.Username!.Equals(username) && x.Password == password).FirstOrDefaultAsync();
            //    return user != null ? Ok(user) : NotFound();
            //}
            //return BadRequest();

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Username!.Equals(username));
                var result = _passwordHasher.Verify(user.Password, password);
                if (user != null && result)
                {
                    var userDTO = new UserDTO
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Password = user.Password,
                        FullName = user.FullName,
                        Email = user.Email,
                        Address = user.Address,
                        DateOfBirth = user.DateOfBirth,
                        Gender = user.Gender,
                        ModifiedDate = user.ModifiedDate,
                        OrderDetails = user.OrderDetails.Select(uo => new OrderDetailDTO
                        {
                            Id = uo.Id,
                            OrderId = uo.OrderId,
                            ProductId = uo.ProductId,
                            Color = uo.Color,
                            Size = uo.Size,
                            Quantity = uo.Quantity,
                            Price = uo.Price,
                            Total = uo.Total,
                        }).ToList(),
                    };
                    return Ok(userDTO);
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }

        // PUT: api/Users/ChangePassword/id
        [HttpPut("ChangePassword/{id}")]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordModel changePasswordModel)
        {
            if (id != changePasswordModel.UserId)
            {
                return BadRequest("User ID in the request body does not match the route parameter.");
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            var result = _passwordHasher.Verify(existingUser.Password, changePasswordModel.OldPassword);
            if (!result)
            {
                return BadRequest("Old password is incorrect");
            }

            //Hash password mới
            existingUser.Password = _passwordHasher.Hash(changePasswordModel.NewPassword);
            _context.Entry(existingUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Password changed successfully.");
        }

        [HttpPut("ResetPassword/{id}")]
        public async Task<IActionResult> ResetPassword(int id, string newPassword)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            existingUser.Password = _passwordHasher.Hash(newPassword);
            _context.Entry(existingUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Password reset successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpGet("GetUserIdByEmail")]
        public async Task<IActionResult> GetUserIdByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                return Ok(user.Id);
            }
            else
            {
                return NotFound("User not found");
            }
        }


        // PUT: api/Users/GenerateOTP/id
        [HttpPut("GenerateOTP/{id}")]
        public async Task<IActionResult> GenerateOTP(int id)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            string otp = GenerateOTPCode();
            existingUser.Otp = otp;
            _context.Entry(existingUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Gửi OTP qua email
            bool emailSent = SendOTPMail(existingUser.Email, otp);
            if (emailSent)
            {
                return Ok("OTP sent successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to send OTP email.");
            }
        }


        // PUT: api/Users/VerifyOTP/id
        [HttpPut("VerifyOTP/{id}")]
        public async Task<IActionResult> VerifyOTP(int id, string enteredOTP)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            if (existingUser.Otp == enteredOTP)
            {
                // Xác minh thành công => thay đổi mật khẩu
                existingUser.Otp = null; // Xóa OTP đã sử dụng
                _context.Entry(existingUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok("OTP verified successfully. User can change the password now.");
            }
            else
            {
                return BadRequest("Invalid OTP.");
            }
        }

        // Tạo mã OTP ngẫu nhiên
        private string GenerateOTPCode()
        {
            //RandomNumberGenerator rng = RandomNumberGenerator.Create();
            //byte[] bytes = new byte[4];
            //rng.GetBytes(bytes);
            //int otpValue = BitConverter.ToInt32(bytes, 0);
            //return otpValue.ToString("D4"); // OTP 4 ký tự

            Random rnd = new Random();
            int otpValue = rnd.Next(100000, 999999);
            return otpValue.ToString();
        }

        // Gửi email chứa OTP
        private bool SendOTPMail(string userEmail, string otp)
        {

            try
            {
                string myEmail = "chatgptquochung@gmail.com";
                string passwordApp = "fbftedckjlvkdoqe";

                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("chatgptquochung@gmail.com");
                mail.To.Add(userEmail);
                mail.Subject = "Vinava";
                mail.Body = $"Vinava would like to inform you that your password has been changed.\r\n" +
                    $"The OTP code to change the password on Vinava App is: {otp}";

                smtpServer.Port = 587;
                smtpServer.EnableSsl = true; // Kết nối bảo mật SSL/TLS
                smtpServer.Credentials = new NetworkCredential(myEmail, passwordApp);
                smtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
