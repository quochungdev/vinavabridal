using CommunityToolkit.Mvvm.ComponentModel;
using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;
public partial class PaymentSuccessPage : ContentPage
{
    private PaymentSuccessPageViewModel _viewmodel;
    public PaymentSuccessPage()
    {
        InitializeComponent();
        _viewmodel = new PaymentSuccessPageViewModel();
        BindingContext = _viewmodel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewmodel.CreateQRCodeCommand.Execute(null);
    }


}