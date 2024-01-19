using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Services
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts();
        Task<ObservableCollection<Product>> SearchProducts(string keyword);
        Task<List<Category>> GetCategories();
        Task<ObservableCollection<Product>> GetProductsByCategoryId(int categoryId);
        Task<ObservableCollection<Product>> GetProductsBySaleDiscount(string selectedSaleDiscount);
        Task<ObservableCollection<Product>> GetProductsByAccessory(string selectedAccessory);
        Task<ObservableCollection<Product>> GetProductsOrderedByPriceAsc();
        Task<ObservableCollection<Product>> GetProductsOrderedByPriceDesc();
        Task<ProductDetail> GetProductById(int productId);
        Task<List<ProductImage>> GetProductsImages(int productId);
        Task<List<Product>> GetProductsIsNew();
        Task<List<Product>> GetInitialProducts();
        Task<List<Product>> GetMoreProducts(int skip, int take, int categoryId = -1, string saleDiscount = null, string keyword = null, string orderBy = null);
    }
}
