using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VinavaFashionProject.DTO;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Services
{
    public class AddressService : IAddressRepository
    {
        JsonSerializerOptions _serializerOptions;
        public AddressService()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<bool> AddAddress(AddressDTO addressDTO)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/Addresses";
                    client.BaseAddress = new Uri(url);
                    string json = JsonSerializer.Serialize(addressDTO, _serializerOptions);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Address>> GetAddressesByUserId(int userId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/Addresses/GetAddressesByUserId/{userId}";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var addresses = await response.Content.ReadFromJsonAsync<List<Address>>();
                        if (addresses != null)
                            return addresses;
                        else
                            throw new ArgumentNullException(nameof(addresses), "Giá trị không thể là null");

                    }
                    else
                    {
                        throw new HttpRequestException($"Error: {response.StatusCode}");
                        return null;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
