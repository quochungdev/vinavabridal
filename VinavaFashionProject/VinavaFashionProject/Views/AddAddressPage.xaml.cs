using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class AddAddressPage : ContentPage
{
    public AddAddressPage(AddressPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}