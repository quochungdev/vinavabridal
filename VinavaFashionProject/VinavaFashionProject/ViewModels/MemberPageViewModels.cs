using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VinavaFashionProject.Models;
using VinavaFashionProject.Views;

namespace VinavaFashionProject.ViewModels;

public partial class MemberPageViewModels : ObservableObject
{
    [ObservableProperty]
    private bool _isLoggedIn;
    [ObservableProperty]
    private bool _isNotLoggedIn;
    [ObservableProperty]
    private string _fullName;
    [ObservableProperty]
    private string _email;


    public MemberPageViewModels()
    {
        if (CheckLoginUser())
        {
            IsLoggedIn = true;
            IsNotLoggedIn = false;

            SetUserData(App.user);
        }
        else
        {
            IsNotLoggedIn = true;
        }
    }

    [ICommand]
    async Task TapLogin()
    {
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    [ICommand]
    async Task GoToSizeInformation()
    {
        if (App.user == null)
        {
            await Shell.Current.DisplayAlert("Notification", "Please Login", "Ok");
            return;
        }
        await Shell.Current.GoToAsync(nameof(SizeInfoPage));
    }

    [ICommand]
    async Task LogOut()
    {
        App.user = null;
        var memberPageVM = new MemberPageViewModels();
        var memberPage = new MemberPage(memberPageVM);
        await Shell.Current.Navigation.PushAsync(memberPage);
    }

    [ICommand]
    async Task GoToAddress()
    {
        if (App.user == null)
        {
            await Shell.Current.DisplayAlert("Notification", "Please Login", "Ok");
            return;
        }
        await Shell.Current.GoToAsync(nameof(AddAddressPage));
    }

    [ICommand]
    async Task GoToFavouriteProduct()
    {
        if (App.user == null)
        {
            await Shell.Current.DisplayAlert("Notification", "Please Login", "Ok");
            return;
        }
        await Shell.Current.GoToAsync(nameof(FavouriteProductPage));
    }

    [ICommand]
    async Task GoToOrderPage()
    {
        if (App.user == null)
        {
            await Shell.Current.DisplayAlert("Notification", "Please Login", "Ok");
            return;
        }
        await Shell.Current.GoToAsync(nameof(OrderPage));
    }

    [ICommand]
    async Task TapRegister()
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }

    [ICommand]
    async Task AccountPage()
    {
        if (App.user == null)
        {
            await Shell.Current.DisplayAlert("Notification", "Please Login", "Ok");
            return;
        }
        await Shell.Current.GoToAsync(nameof(AccountInfoPage));
    }

    private void SetUserData(User user)
    {
        FullName = user.FullName;
        Email = user.Email;
    }

    private bool CheckLoginUser()
    {
        if (App.user != null)
        {
            return true;
        }
        else return false;
    }


}