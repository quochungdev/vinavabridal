using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class PaymentPayPalSuccessPage : ContentPage
{
    public PaymentPayPalSuccessPage(PaymentSuccessPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}