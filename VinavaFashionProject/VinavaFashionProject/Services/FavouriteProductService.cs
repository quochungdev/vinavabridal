using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Services
{
    public class FavouriteProductService : IFavouriteProductRepository
    {
        JsonSerializerOptions _serializerOptions;
        public FavouriteProductService()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        public async Task<List<FavouriteProduct>> GetFavouriteProductsByUserId(int userId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/FavoriteProducts/user/{userId}";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);

                    if (response.IsSuccessStatusCode)
                    {
                        var fpList = await response.Content.ReadFromJsonAsync<List<FavouriteProduct>>();
                        return fpList;
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<FavouriteProduct> GetFavouriteProductsByUserIdAndProductId(int productId, int userId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/FavoriteProducts/user/{userId}/{productId}";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        FavouriteProduct fp = await response.Content.ReadFromJsonAsync<FavouriteProduct>();
                        return fp;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public async Task<bool> PostFavouriteProduct(FavouriteProductDTO favouriteProduct)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/FavoriteProducts";
                    client.BaseAddress = new Uri(url);
                    string json = JsonSerializer.Serialize(favouriteProduct, _serializerOptions);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        await HandleUnsuccessfulResponse(response);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteFavouriteProduct(int userId, int productId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/FavoriteProducts/user/{userId}/product/{productId}";
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
