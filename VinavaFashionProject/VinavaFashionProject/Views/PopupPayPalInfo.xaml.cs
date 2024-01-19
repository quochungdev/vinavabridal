using CommunityToolkit.Maui.Views;
using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class PopupPayPalInfo : Popup
{
    public PopupPayPalInfo(PopupPayPalInfoViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}