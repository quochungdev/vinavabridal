using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;

namespace VinavaFashionProject.ViewModels;

public partial class HomePageViewModels : ObservableObject
{
    readonly IPayPalRepository _payPalService = new PayPalService();
    readonly IProductRepository _productService = new ProductService();
    PayPalAccessToken result;
    [ObservableProperty]
    private ObservableCollection<Product> _products;

    [ObservableProperty]
    public ObservableCollection<string> _carouselList;

    public HomePageViewModels()
    {
        _products = new ObservableCollection<Product>();
        CarouselList = new ObservableCollection<string>
            {
                "v1.png"
            };
    }

    [ICommand]
    public async Task Initialize()
    {
        //_products = new ObservableCollection<Product>();
        //CarouselList = new ObservableCollection<string>
        //    {
        //        "v1.png"
        //    };
        await LoadProducts();
    }


    [ICommand]
    public async Task GoToProductPage()
    {
        await Shell.Current.GoToAsync($"//ProductPage");
    }

    [ICommand]
    private async Task LoadProducts()
    {

        //Products.Clear();
        List<Product> products = await _productService.GetProductsIsNew();
        if (products != null)
        {
            foreach (var product in products)
            {
                UpdateImageSource(product);
            }
            Shell.Current.Dispatcher.Dispatch(() =>
            {
                Products = new ObservableCollection<Product>(products);
            });

        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Can not find Products", "Ok");
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

    [ICommand]
    private async Task GoToCart()
    {
        if (App.user != null)
        {
            await Shell.Current.GoToAsync(nameof(CartPage));
        }
        else
            await Shell.Current.DisplayAlert("Notification", "Please login", "Ok");
        return;

    }
}