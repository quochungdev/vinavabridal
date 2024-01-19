using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Services
{
    public interface IFavouriteProductRepository
    {
        Task<List<FavouriteProduct>> GetFavouriteProductsByUserId(int userId);
        Task<bool> PostFavouriteProduct(FavouriteProductDTO favouriteProduct);
        Task<FavouriteProduct> GetFavouriteProductsByUserIdAndProductId(int productId, int userId);
        Task<bool> DeleteFavouriteProduct(int userId, int productId);
    }
}
