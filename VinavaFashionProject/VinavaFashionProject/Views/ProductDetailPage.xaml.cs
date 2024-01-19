using CommunityToolkit.Maui.Views;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using VinavaFashionProject.Models;
using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;
public partial class ProductDetailPage : ContentPage
{
    private ProductDetailPageViewModel _viewmodel;
    private bool isHeartClicked = false;

    public ProductDetailPage()
    {
        InitializeComponent();
        _viewmodel = new ProductDetailPageViewModel();
        BindingContext = _viewmodel;

    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewmodel.LoadFavouriteCommand.ExecuteAsync(null);
        await _viewmodel.InitializeCommand.ExecuteAsync(null);
        if (_viewmodel.FavouriteProdId == _viewmodel.ProductId && App.user.Id == _viewmodel.FavouriteProd.UserId)
        {
            heartButton.Icon = "heart_click_32";
            heartButton.IconColor = Color.FromRgba(255, 0, 0, 255);
        }

        else
        {
            heartButton.Icon = "heart_32";
            heartButton.IconColor = Color.FromRgba(0, 0, 0, 255);
        }

        _viewmodel.ResetDataCommand.Execute(null);
    }

    private void TapColorClick(object sender, EventArgs e)
    {
        if (sender is BoxView boxView && boxView.BindingContext is ProductAttribute attribute)
        {
            foreach (var item in _viewmodel.ListColor)
            {
                if (item != attribute)
                {
                    item.IsVisibleBorder = false;
                }
            }
            attribute.IsVisibleBorder = true;
            _viewmodel.SelectedColorExecuteCommand.Execute(attribute);
        }
    }

    private void OnButtonTapped(object sender, EventArgs e)
    {
        if (sender is Button clickedBtn && clickedBtn.BindingContext is ProductAttribute clickedAttribute)
        {
            foreach (var item in _viewmodel.ListSize)
            {
                if (item != clickedAttribute)
                {
                    item.BorderWidth = 0;
                }
            }
            clickedAttribute.BorderWidth = 3;
            _viewmodel.SelectedSizeExecuteCommand.Execute(clickedAttribute);
        }
    }

    private void OnPopupClicked(object sender, EventArgs e)
    {
        if (BindingContext is ProductDetailPageViewModel vm)
        {
            var popupViewModel = new PopupImagesViewModel(vm.ProductImages);
            var popup = new PopupImages(popupViewModel);
            this.ShowPopup(popup);
        }
    }

    private async void OnHeartButtonClicked(object sender, EventArgs e)
    {
        if (App.user == null)
        {
            await Shell.Current.DisplayAlert("Notification", "Vui lòng đăng nhập", "OK");
            return;
        }

        if (_viewmodel.FavouriteProd?.ProductId == _viewmodel.ProductId && App.user.Id == _viewmodel.FavouriteProd.UserId)
        {
            heartButton.Icon = "heart_32";
            heartButton.IconColor = Color.FromRgba(0, 0, 0, 255);
            await _viewmodel.DeleteFavouriteProductCommand.ExecuteAsync(null);
        }
        else
        {
            heartButton.Icon = "heart_click_32";
            heartButton.IconColor = Color.FromRgba(255, 0, 0, 255);
            await _viewmodel.AddToFavouriteCommand.ExecuteAsync(null);
        }

        isHeartClicked = !isHeartClicked;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}