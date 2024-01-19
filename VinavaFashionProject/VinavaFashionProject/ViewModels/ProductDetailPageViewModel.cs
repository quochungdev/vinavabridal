using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using VinavaFashionProject.Api.DTO;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;

namespace VinavaFashionProject.ViewModels;
[QueryProperty(nameof(ProductId), nameof(ProductId))]
public partial class ProductDetailPageViewModel : ObservableObject
{
    readonly IProductRepository _productService = new ProductService();
    readonly IOrderDetailRepository _orderDetailService = new OrderDetailService();
    private readonly IFavouriteProductRepository _favouriteProductService = new FavouriteProductService();

    [ObservableProperty]
    private ProductDetail _productDetailVM;

    [ObservableProperty]
    private List<ProductImage> _productImages;

    [ObservableProperty]
    private int _productId;

    [ObservableProperty]
    private ObservableCollection<Product> _productsByCategory;

    [ObservableProperty]
    private List<ProductAttribute> _productAttributes;

    [ObservableProperty]
    private List<ProductAttribute> _listSize;

    [ObservableProperty]
    private List<ProductAttribute> _listColor;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    public bool _isContentVisible;

    [ObservableProperty]
    private string _selectedColor;

    [ObservableProperty]
    private string _selectedSize;

    [ObservableProperty]
    private int _quantity = 1;

    [ObservableProperty]
    private int _favouriteProdId;

    [ObservableProperty]
    private FavouriteProduct _favouriteProd;
    public ProductDetailPageViewModel()
    {

    }

    [ICommand]
    public async Task InitializeAsync()
    {

        await LoadProductDetailAndImages();
        //await LoadProductsByCategory();

    }

    [ICommand]
    private async Task LoadFavourite()
    {
        if (App.user == null)
        {
            return;
        }
        FavouriteProduct fp = await _favouriteProductService.GetFavouriteProductsByUserIdAndProductId(ProductId, App.user.Id);
        if (fp == null)
        {
            return;
        }
        FavouriteProdId = fp.ProductId;
        FavouriteProd = fp;
    }

    private async Task LoadProductsByCategory()
    {
        IsBusy = true;
        await Task.Run(async () =>
        {
            var productsByCategoryId = await _productService.GetProductsByCategoryId(ProductDetailVM.CategoryId);
            foreach (var product in productsByCategoryId)
            {
                await UpdateImageSourceProduct(product);
            }
            ProductsByCategory = new ObservableCollection<Product>(productsByCategoryId);
        });
        IsBusy = false;
    }

    public async Task LoadProductDetailAndImages()
    {
        try
        {
            IsBusy = true;
            IsContentVisible = false;

            await Task.Run(async () =>
            {
                if (ProductId != 0)
                {
                    ClearPreviousData();
                    ProductDetail productDetail = await _productService.GetProductById(ProductId);
                    ProductDetailVM = productDetail;

                    List<ProductImage> productImages = await _productService.GetProductsImages(ProductId);
                    foreach (var product in productImages)
                    {
                        await UpdateImageSource(product);
                    }
                    ProductImages = new List<ProductImage>(productImages);

                    List<ProductAttribute> colors = LoadColors(productDetail);
                    List<ProductAttribute> sizes = LoadSizes(productDetail);
                    ListColor = new List<ProductAttribute>(colors);
                    ListSize = new List<ProductAttribute>(sizes);
                }
            });
            IsBusy = false;
            IsContentVisible = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    //public async Task LoadImages()
    //{
    //    List<ProductImage> productImages = await _productService.GetProductsImages(ProductId);
    //    foreach (var product in productImages)
    //    {
    //        UpdateImageSource(product);
    //    }
    //    ProductImages = new List<ProductImage>(productImages);
    //}

    public List<ProductAttribute> LoadColors(ProductDetail productDetail)
    {
        return productDetail.ProductAttributes.Where(attr => attr.Attribute?.AttributeName == "color").ToList();
    }

    public List<ProductAttribute> LoadSizes(ProductDetail productDetail)
    {
        return productDetail.ProductAttributes.Where(attr => attr.Attribute?.AttributeName == "size").ToList();
    }

    //private List<ProductImage> GetProductImages(ProductDetail productDetail)
    //{
    //    return productDetail.ProductImages.ToList();
    //}

    [ICommand]
    public async Task AddToFavourite()
    {
        FavouriteProductDTO fpDTO = new FavouriteProductDTO
        {
            UserId = App.user.Id,
            ProductId = ProductId,
            FavoriteDate = DateTime.Now
        };

        bool result = await _favouriteProductService.PostFavouriteProduct(fpDTO);
        if (result)
        {
            FavouriteProd = new FavouriteProduct
            {
                UserId = App.user.Id,
                ProductId = ProductId,
                FavoriteDate = DateTime.Now
            };

            await Shell.Current.DisplayAlert("Notification", "Added product to Favorite Products list", "Ok");
        }
    }

    [ICommand]
    private async Task DeleteFavouriteProduct()
    {
        if (App.user == null)
        {
            return;
        }

        bool result = await _favouriteProductService.DeleteFavouriteProduct(App.user.Id, ProductId);
        if (result)
        {
            FavouriteProd = null;
            await Shell.Current.DisplayAlert("Notification", "Removed from favorites list", "Ok");
        }
    }
    private async Task UpdateImageSourceProduct(Product product)
    {
        await Task.Run(() =>
        {
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                byte[] imageBytes = Convert.FromBase64String(product.ImageUrl);
                ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));

                product.ImageSourceData = imageSource;
            }
        });
    }
    private async Task UpdateImageSource(ProductImage pImage)
    {
        await Task.Run(() =>
        {
            if (!string.IsNullOrEmpty(pImage.ImageUrl))
            {
                byte[] imageBytes = Convert.FromBase64String(pImage.ImageUrl);
                ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));

                pImage.ImageSourceData = imageSource;
            }
        });
    }
    private void ClearPreviousData()
    {
        ProductDetailVM = null;
        ProductImages = null;
    }

    [ICommand]
    private void TapColor()
    {
        foreach (var attribute in ProductAttributes.Where(a => a.IsVisibleBorder == true))
        {
            attribute.IsVisibleBorder = false;
        }
        OnPropertyChanged(nameof(ProductAttributes));
    }

    [ICommand]
    private void SelectedColorExecute(ProductAttribute attribute)
    {
        if (attribute?.Attribute?.AttributeName == "color")
        {
            SelectedColor = attribute.Attribute.AttributeValue;
        }
    }

    [ICommand]
    private void SelectedSizeExecute(ProductAttribute attribute)
    {
        if (attribute?.Attribute?.AttributeName == "size")
        {
            SelectedSize = attribute.Attribute.AttributeValue;
        }
    }

    [ICommand]
    private void IncreaseQuantity()
    {
        Quantity++;
    }
    [ICommand]
    private void DecreaseQuantity()
    {
        if (Quantity > 1)
        {
            Quantity--;
        }
    }

    [ICommand]
    private async void AddToCart()
    {
        if (App.user == null)
        {
            await Shell.Current.DisplayAlert("Notification", "Please login", "OK");
            return;
        }
        OrderDetailDTO orderDetailDTO = new OrderDetailDTO
        {
            UserId = App.user.Id,
            Color = SelectedColor,
            Size = SelectedSize,
            ProductId = ProductId,
            Price = ProductDetailVM.Price,
            Quantity = Quantity,
            Total = ProductDetailVM.Price * Quantity
        };
        bool success = await _orderDetailService.AddOrderDetail(orderDetailDTO);
        if (success)
        {
            string result = await Shell.Current.DisplayActionSheet("Success", null, null, "Go to cart", "Continue shopping");
            if (result == "Go to cart")
            {
                await Shell.Current.Navigation.PopToRootAsync();
                await Shell.Current.GoToAsync("//HomePage");
                await Shell.Current.GoToAsync(nameof(CartPage));
            }
            else if (result == "Continue shopping")
            {
                return;
            }
        }
        else if (success == false && SelectedColor.IsNullOrEmpty())
        {
            await Shell.Current.DisplayAlert("Notification", "Please select Color", "OK");
            return;
        }
        else if (success == false && SelectedSize.IsNullOrEmpty())
        {
            await Shell.Current.DisplayAlert("Notification", "Please select Size", "OK");
            return;
        }
    }

    [ICommand]
    private void ResetData()
    {
        SelectedColor = string.Empty;
        SelectedSize = string.Empty;
        Quantity = 1;
    }


}