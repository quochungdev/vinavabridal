using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;

namespace VinavaFashionProject.ViewModels
{
    public partial class FavouriteProductPageViewModel : ObservableObject
    {
        readonly IFavouriteProductRepository _favouriteProductService = new FavouriteProductService();

        [ObservableProperty]
        private ObservableCollection<FavouriteProduct> _favouriteProducts;

        public FavouriteProductPageViewModel()
        {

        }
        public async Task InitializeAsync()
        {
            FavouriteProducts = new ObservableCollection<FavouriteProduct>();
            await LoadFavouriteProductByUserId();
        }

        [ICommand]
        private async Task LoadFavouriteProductByUserId()
        {
            if (App.user == null)
            {
                return;
            }
            List<FavouriteProduct> fpByUserId = await _favouriteProductService.GetFavouriteProductsByUserId(App.user.Id);
            if (fpByUserId != null)
            {
                FavouriteProducts.Clear();
                foreach (var fb in fpByUserId)
                {
                    foreach (var product in fpByUserId)
                    {
                        UpdateImageSource(product.Product);
                    }

                    FavouriteProducts.Add(fb);
                }
            }
        }

        private void UpdateImageSource(Product product)
        {
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                byte[] imageBytes = Convert.FromBase64String(product.ImageUrl);
                ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));

                product.ImageSourceData = imageSource;
            }
        }
    }
}
