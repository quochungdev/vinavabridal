using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class OrderDetailPage : ContentPage
{
    private OrderDetailPageViewModel _viewmodel;

    public OrderDetailPage()
    {
        InitializeComponent();
        _viewmodel = new OrderDetailPageViewModel();
        BindingContext = _viewmodel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewmodel.InitializeAsync();
    }
}