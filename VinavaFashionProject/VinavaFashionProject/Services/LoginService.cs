using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VinavaFashionProject.DTO;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Services
{
    public class LoginService : ILoginRepository
    {
        JsonSerializerOptions _serializerOptions;

        public LoginService()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        public async Task<User> Login(string username, string password)
        {
            try
            {
                var client = new HttpClient();
                string localhostUrl = $"{ServiceCommon.BaseAddress}/api/user/login/" + username + "/" + password;
                client.BaseAddress = new Uri(localhostUrl);
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                if (response.IsSuccessStatusCode)
                {
                    User user = await response.Content.ReadFromJsonAsync<User>();
                    return await Task.FromResult(user);
                }
                //else
                //    await HandleUnsuccessfulResponse(response);

                return null;
            }
            catch (Exception ex)
            {
                //await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                return null;
            }
        }
        public async Task<bool> CreateUser(User user)
        {
            try
            {
                var client = new HttpClient();
                string localhostUrl = $"{ServiceCommon.BaseAddress}/api/user/create";
                client.BaseAddress = new Uri(localhostUrl);

                string json = System.Text.Json.JsonSerializer.Serialize<User>(user, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Username or Email is already in use", "Ok");
                    return false;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                return false;
            }
        }
        public async Task<UserDTO> UpdateUser(int id, UserDTO userDTO)
        {
            try
            {
                var client = new HttpClient();
                string localhostUrl = $"{ServiceCommon.BaseAddress}/api/user/{App.user.Id}";
                client.BaseAddress = new Uri(localhostUrl);

                string json = System.Text.Json.JsonSerializer.Serialize(userDTO);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(client.BaseAddress, content);
                if (response.IsSuccessStatusCode)
                {
                    UserDTO updateUserData = await response.Content.ReadFromJsonAsync<UserDTO>();
                    return updateUserData;
                }
                else
                {
                    await HandleUnsuccessfulResponse(response);
                    return null;
                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
                return null;
            }
        }
        public async Task<string> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            try
            {
                var client = new HttpClient();
                string localhostUrl = $"{ServiceCommon.BaseAddress}/api/user/ChangePassword/{userId}";
                client.BaseAddress = new Uri(localhostUrl);

                var changePasswordModel = new
                {
                    UserId = userId,
                    OldPassword = oldPassword,
                    NewPassword = newPassword
                };

                string json = System.Text.Json.JsonSerializer.Serialize(changePasswordModel);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(client.BaseAddress, content);

                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Notification", "Change Password Successfully", "OK");
                    return "Password changed successfully.";
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return $"Failed to change password. Status code: {response.StatusCode}. Message: {errorMessage}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        public async Task<string> ResetPassword(int userId, string newPassword)
        {
            var client = new HttpClient();
            string localhostUrl = $"{ServiceCommon.BaseAddress}/api/user/ResetPassword/{userId}?newPassword={newPassword}";
            client.BaseAddress = new Uri(localhostUrl);
            HttpResponseMessage response = await client.PutAsync(client.BaseAddress, null);
            if (response.IsSuccessStatusCode)
            {
                await Shell.Current.DisplayAlert("Notification", "Reset password Successfully", "OK");
                return "Reset password Successfully";
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Reset password Failed, Please try again", "OK");
                return "Reset password Failed";
            }
        }
        public async Task<string> VerifyOTP(int userId, string enteredOTP)
        {
            try
            {
                var client = new HttpClient();
                string localhostUrl = $"{ServiceCommon.BaseAddress}/api/user/VerifyOTP/{userId}?enteredOTP={enteredOTP}";
                client.BaseAddress = new Uri(localhostUrl);

                HttpResponseMessage response = await client.PutAsync(client.BaseAddress, null);

                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Notification", "Verify OTP Successfully", "OK");
                    return "Verify OTP Successfully";
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Error Verify OTP. Please try again", "OK");
                    return "Error Verify OTP";
                }

            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

        }

        public async Task<int?> GetUserIdByEmail(string email)
        {
            try
            {
                var client = new HttpClient();
                string url = $"{ServiceCommon.BaseAddress}/api/user/GetUserIdByEmail?email={email}";
                client.BaseAddress = new Uri(url);

                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                if (response.IsSuccessStatusCode)
                {
                    int userId = await response.Content.ReadFromJsonAsync<int>();
                    return userId;
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to get user ID by email. Status code: {response.StatusCode}. Message: {errorMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        public async Task<User> GetUserById(int id)
        {
            try
            {
                var client = new HttpClient();
                string url = $"{ServiceCommon.BaseAddress}/api/user/{id}";
                client.BaseAddress = new Uri(url);

                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                if (response.IsSuccessStatusCode)
                {
                    User user = await response.Content.ReadFromJsonAsync<User>();
                    return user;
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to get user ID by email. Status code: {response.StatusCode}. Message: {errorMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string> SendOTP(int userId)
        {
            try
            {
                var client = new HttpClient();
                string localhostUrl = $"{ServiceCommon.BaseAddress}/api/user/GenerateOTP/{userId}";
                client.BaseAddress = new Uri(localhostUrl);

                HttpResponseMessage response = await client.PutAsync(client.BaseAddress, null);
                if (response.IsSuccessStatusCode)
                {
                    return "OTP sent successfully";
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    return $"Failed to send OTP. Status code: {response.StatusCode}. Message: {errorMessage}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

        }

        private async Task HandleUnsuccessfulResponse(HttpResponseMessage response)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();

            await Shell.Current.DisplayAlert("Error", $"Failed with status code {response.StatusCode}. Message: {errorMessage}", "Ok");
        }
    }
}
