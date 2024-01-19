using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}