using VinavaFashionProject.Models;
using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class CartPage : ContentPage
{
    private CartPageViewModel _viewmodel;

    public CartPage()
    {
        InitializeComponent();
        _viewmodel = new CartPageViewModel();
        BindingContext = _viewmodel;
    }

    public void SelectedSizeChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker && picker.SelectedItem is ProductAttribute selectedSize)
        {
            if (BindingContext is CartPageViewModel viewModel)
            {
                foreach (var orderDetail in viewModel.OrderDetails)
                {
                    if (orderDetail.SelectedSize.Id == selectedSize.Id)
                    {
                        orderDetail.Size = selectedSize.Attribute.AttributeValue;
                        _viewmodel.ChangeSizeCommand.Execute(orderDetail);
                        break;
                    }
                }
            }
        }
    }

    public void SelectedColorChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker && picker.SelectedItem is ProductAttribute selectedColor)
        {
            if (BindingContext is CartPageViewModel viewModel)
            {
                foreach (var orderDetail in viewModel.OrderDetails)
                {
                    if (orderDetail.SelectedColor.Id == selectedColor.Id)
                    {
                        orderDetail.Color = selectedColor.Attribute.AttributeValue;
                        _viewmodel.ChangeColorCommand.Execute(orderDetail);
                        break;
                    }
                }
            }
        }
    }

    private void OnInTheCountryCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _viewmodel.TransportFee = 0;
            _viewmodel.TotalAmount = _viewmodel.TotalProduct + _viewmodel.TransportFee;
            _viewmodel.IsSelectedMethodFee = true;
        }
    }
    private void OnForeignCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _viewmodel.TransportFee = 59;
            _viewmodel.TotalAmount = _viewmodel.TotalProduct + _viewmodel.TransportFee;
            _viewmodel.IsSelectedMethodFee = true;
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewmodel.OrderDetails == null || !_viewmodel.OrderDetails.Any())
        {
            await _viewmodel.InitializeAsync();
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }

}