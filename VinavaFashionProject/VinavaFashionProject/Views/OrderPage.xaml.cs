using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class OrderPage : ContentPage
{
    private OrderPageViewModel _viewmodel;

    public OrderPage()
	{
		InitializeComponent();
        _viewmodel = new OrderPageViewModel();
        BindingContext = _viewmodel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewmodel.Orders == null || !_viewmodel.Orders.Any())
        {
                await _viewmodel.InitializeAsync();
        }
    }
}