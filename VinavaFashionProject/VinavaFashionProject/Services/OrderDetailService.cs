using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VinavaFashionProject.Api.DTO;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Services
{
    public class OrderDetailService : IOrderDetailRepository
    {
        JsonSerializerOptions _serializerOptions;
        public OrderDetailService()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<bool> AddOrderDetail(OrderDetailDTO orderDetailDTO)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (!orderDetailDTO.Size.IsNullOrEmpty() && !orderDetailDTO.Color.IsNullOrEmpty())
                    {
                        string url = $"{ServiceCommon.BaseAddress}/api/orderdetails";
                        client.BaseAddress = new Uri(url);
                        string json = JsonSerializer.Serialize(orderDetailDTO, _serializerOptions);
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
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }
        public async Task<List<OrderDetail>> GetOrderDetailsByUserId(int userId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/OrderDetails/GetOrderDetailsCartByUserId/{userId}";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var orderDetails = await response.Content.ReadFromJsonAsync<List<OrderDetail>>();
                        if (orderDetails != null)
                            return orderDetails;
                        else
                            throw new ArgumentNullException(nameof(orderDetails), "Giá trị không thể là null");

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

        public List<ProductAttribute> GetProductAttributesByProductIdAndAttributeName(List<OrderDetail> orderDetails, int productId, string attributeName)
        {
            List<ProductAttribute> productAttributes = orderDetails
                .Where(od => od.ProductId == productId)
                .SelectMany(od => od.Product?.ProductAttributes.Where(pa => pa.Attribute?.AttributeName == attributeName))
                .ToList();

            return productAttributes ?? new List<ProductAttribute>();
        }

        public async Task<bool> UpdateColor(int id, string newColor)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/OrderDetails/UpdateColor/{id}?newColor={newColor}";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.PutAsync(client.BaseAddress, null);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateSize(int id, string newSize)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/OrderDetails/UpdateSize/{id}?newSize={newSize}";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.PutAsync(client.BaseAddress, null);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateQuantity(int id, int newQuantity)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/OrderDetails/UpdateQuantity/{id}?newQuantity={newQuantity}";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.PutAsync(client.BaseAddress, null);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteOrderDetailAsync(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/OrderDetails/{id}";
                    client.BaseAddress = new Uri(url);
                    HttpResponseMessage response = await client.DeleteAsync(client.BaseAddress);
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
        private async Task HandleUnsuccessfulResponse(HttpResponseMessage response)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();

            await Shell.Current.DisplayAlert("Error", $"Failed with status code {response.StatusCode}. Message: {errorMessage}", "Ok");
        }
    }
}
