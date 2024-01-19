using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class ResetPasswordPage : ContentPage
{
    public ResetPasswordPage(VerifyOTPPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}