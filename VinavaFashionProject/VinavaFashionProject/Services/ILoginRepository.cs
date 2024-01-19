using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.DTO;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Services
{
    public interface ILoginRepository
    {
        Task<User> Login(string username, string password);
        Task<bool> CreateUser(User user);
        Task<UserDTO> UpdateUser(int id, UserDTO userDTO);
        Task<string> ChangePassword(int userId, string oldPassword, string newPassword);
        Task<string> SendOTP(int userId);
        Task<string> VerifyOTP(int userId, string enteredOTP);
        Task<int?> GetUserIdByEmail(string email);
        Task<User> GetUserById(int id);
        Task<string> ResetPassword(int userId, string newPassword);
    }
}
