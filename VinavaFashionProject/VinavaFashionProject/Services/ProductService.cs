using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Services
{
    public class ProductService : IProductRepository
    {
        JsonSerializerOptions _serializerOptions;
        public ProductService()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        public async Task<List<Product>> GetProducts()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/products";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                        if (products != null)
                            return products;
                        else
                            throw new ArgumentNullException(nameof(products), "Giá trị không thể là null");

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

        public async Task<List<Product>> GetInitialProducts()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/products/initial";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                        if (products != null)
                            return products;
                        else
                            throw new ArgumentNullException(nameof(products), "Giá trị không thể là null");

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

        public async Task<List<Product>> GetMoreProducts(int skip, int take, int categoryId = -1, string saleDiscount = null, string keyword = null, string orderBy = null)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/products/morefiltered?skip={skip}&take={take}";
                    if (!string.IsNullOrEmpty(orderBy))
                    {
                        url += $"&orderBy={orderBy}";
                    }

                    if (categoryId != -1)
                    {
                        url += $"&categoryId={categoryId}";
                    }

                    if (!string.IsNullOrEmpty(saleDiscount))
                    {
                        url += $"&saleDiscount={saleDiscount}";
                    }

                    if (!string.IsNullOrEmpty(keyword))
                    {
                        url += $"&keyword={keyword}";
                    }
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                        if (products != null)
                            return products;
                        else
                            throw new ArgumentNullException(nameof(products), "Giá trị không thể là null");

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

        public async Task<List<Product>> GetProductsIsNew()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/products/IsNew";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                        if (products != null)
                            return products;
                        else
                            throw new ArgumentNullException(nameof(products), "Giá trị không thể là null");

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

        public async Task<ObservableCollection<Product>> GetProductsByCategoryId(int categoryId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/products/category/{categoryId}/products";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var products = await response.Content.ReadFromJsonAsync<ObservableCollection<Product>>();
                        if (products != null)
                            return products;
                        else
                            return new ObservableCollection<Product>();

                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "There are no products in this category", "Ok");
                        return new ObservableCollection<Product>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ObservableCollection<Product>();
            }
        }

        public async Task<ProductDetail> GetProductById(int productId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/products/{productId}";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        ProductDetail product = await response.Content.ReadFromJsonAsync<ProductDetail>();
                        return product;
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

        public async Task<ObservableCollection<Product>> GetProductsBySaleDiscount(string selectedSaleDiscount)
        {
            List<Product> allProducts = await GetProducts();
            if (allProducts == null)
            {
                return new ObservableCollection<Product>();
            }
            var list = allProducts.Where(p => p.SaleDiscountString == selectedSaleDiscount).Take(20).ToList();
            var observableList = new ObservableCollection<Product>(list);
            return observableList;
        }

        public async Task<ObservableCollection<Product>> GetProductsByAccessory(string selectedAccessory)
        {
            List<Product> allProducts = await GetProducts();
            List<Category> allCategories = await GetCategories();
            if (allProducts == null)
            {
                return new ObservableCollection<Product>();
            }
            var list = allProducts.Join(allCategories,
                product => product.CategoryId,
               category => category.Id, (product, category)
               => new { Product = product, Category = category })
                .Where(p => p.Category.IsAccessory && p.Category.IsAccessory == true)
                .Select(p => p.Product).Take(20)
                .ToList();

            var observableList = new ObservableCollection<Product>(list);
            return observableList;
        }

        public async Task<ObservableCollection<Product>> GetProductsOrderedByPriceAsc()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/products/PriceAsc";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var products = await response.Content.ReadFromJsonAsync<ObservableCollection<Product>>();
                        if (products != null)
                            return products;
                        else
                            throw new ArgumentNullException(nameof(products), "Giá trị không thể là null");

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

        public async Task<ObservableCollection<Product>> GetProductsOrderedByPriceDesc()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/products/PriceDesc";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var products = await response.Content.ReadFromJsonAsync<ObservableCollection<Product>>();
                        if (products != null)
                            return products;
                        else
                            throw new ArgumentNullException(nameof(products), "Giá trị không thể là null");

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

        public async Task<List<ProductImage>> GetProductsImages(int productId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/products/{productId}/ProductDetailImages";
                    client.BaseAddress = new Uri(url);

                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var productImages = await response.Content.ReadFromJsonAsync<List<ProductImage>>();
                        return productImages;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new Exception($"Không tìm thấy hình ảnh cho sản phẩm có Id là {productId}");
                    }
                    else
                    {
                        throw new Exception("Lỗi khi gửi yêu cầu lấy hình ảnh từ server.");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Lỗi khi kết nối đến server: " + ex.Message);
            }
        }

        public async Task<ObservableCollection<Product>> SearchProducts(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                throw new ArgumentException("Keyword không hợp lệ");
            }

            using (var client = new HttpClient())
            {
                string url = $"{ServiceCommon.BaseAddress}/api/products/search?keyword={keyword}";
                client.BaseAddress = new Uri(url);
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Can not find Products");
                }
                var products = await response.Content.ReadFromJsonAsync<ObservableCollection<Product>>();
                return products;
            }
        }

        public async Task<List<Category>> GetCategories()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{ServiceCommon.BaseAddress}/api/categories";
                    client.BaseAddress = new Uri(url);
                    HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                    if (response.IsSuccessStatusCode)
                    {
                        var categories = await response.Content.ReadFromJsonAsync<List<Category>>();
                        Console.WriteLine(categories);
                        return categories;
                    }
                    else
                    {
                        // Xử lý lỗi nếu có
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
