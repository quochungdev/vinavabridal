using VinavaFashionProject.Models;
using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class FavouriteProductPage : ContentPage
{
    private FavouriteProductPageViewModel _viewmodel;

    public FavouriteProductPage()
	{
		InitializeComponent();
        _viewmodel = new FavouriteProductPageViewModel();
        BindingContext = _viewmodel;
    }

    private async void OnTappedProductDetail(object sender, EventArgs e)
    {
        if (sender is Label label && label.BindingContext is FavouriteProduct fp)
        {
            int ProductId = fp.ProductId;
            if (ProductId != null)
            {
                await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?ProductId={ProductId}");
                //await _viewmodel.TappedProductDetailsCommand.ExecuteAsync(product.Id);
            }
        }
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewmodel.InitializeAsync();
    }
}