using Newtonsoft.Json;
using QRCoder;
using System.Drawing;
using System.IO;
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
    public class OrderService : IOrderRepository
    {
        JsonSerializerOptions _serializerOptions;
        public OrderService()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<int> CreateOrder(OrderDTO order)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/orders";
                    client.BaseAddress = new Uri(url);
                    string json = System.Text.Json.JsonSerializer.Serialize(order, _serializerOptions);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content);
                    if (response.IsSuccessStatusCode)
                    {
                        int orderId = await response.Content.ReadFromJsonAsync<int>();
                        return orderId;
                    }
                    else
                    {
                        //await HandleUnsuccessfulResponse(response);
                        await Shell.Current.DisplayAlert("Notification", "Vui lòng chọn đầy đủ thông tin", "0k");
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return 0;
            }
        }

        public async Task<Order> GetOrdersById(int orderId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/Orders/{orderId}";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        Order order = await response.Content.ReadFromJsonAsync<Order>();
                        return order;

                    }
                    else
                    {
                        throw new HttpRequestException($"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/Orders/User/{userId}";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var orders = await response.Content.ReadFromJsonAsync<List<Order>>();
                        if (orders != null)
                            return orders;
                        else
                            throw new ArgumentNullException(nameof(orders), "Giá trị không thể là null");

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

        public async Task<ApiResponse> GenerateQRCode(decimal enterAmount, string enterInfo)
        {
            var requestData = new
            {
                acqId = 970405,
                accountNo = 6702295109403,
                accountName = "DU QUOC HUNG",
                amount = enterAmount,
                format = "text",
                addInfo = enterInfo,
                template = "print",
            };

            var jsonRequest = JsonConvert.SerializeObject(requestData);

            try
            {
                using (var client = new HttpClient())
                {
                    var requestContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("https://api.vietqr.io/v2/generate", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var dataResult = JsonConvert.DeserializeObject<ApiResponse>(content);
                        return dataResult;
                    }
                    else
                    {
                        await HandleUnsuccessfulResponse(response);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }
        public async Task<decimal> GetExchangeRate(string currency)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/Orders/GetExchangeRate?currency={currency}";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var exchangeRate = await response.Content.ReadFromJsonAsync<decimal>();
                        return exchangeRate;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new Exception("Exchange rate not found or invalid for the given currency.");
                    }
                    else
                    {
                        throw new Exception("Error fetching exchange rate.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching exchange rate.");
            }
        }

        public async Task<BankAccount> GetBankAccount()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/PayPalDbs/BankAccount";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var BankAccount = await response.Content.ReadFromJsonAsync<BankAccount>();
                        return BankAccount;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new Exception("not found or invalid.");
                    }
                    else
                    {
                        throw new Exception("Error fetching exchange rate.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching exchange rate.");
            }
        }
        public async Task<PayPalDb> GetPayPalDB()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/PayPalDbs/data";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var payPalDb = await response.Content.ReadFromJsonAsync<PayPalDb>();
                        return payPalDb;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new Exception("not found or invalid.");
                    }
                    else
                    {
                        throw new Exception("Error fetching exchange rate.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching exchange rate.");
            }
        }
        private async Task HandleUnsuccessfulResponse(HttpResponseMessage response)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();

            await Shell.Current.DisplayAlert("Error", $"Failed with status code {response.StatusCode}. Message: {errorMessage}", "Ok");
        }

    }
}