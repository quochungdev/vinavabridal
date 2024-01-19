using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Windows.Input;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;
namespace VinavaFashionProject.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;

    private ICommand _signInCommand;

    public ICommand SignInCommand => _signInCommand ??= new RelayCommand(SignIn);

    readonly ILoginRepository loginService = new LoginService();
    readonly IPasswordHasher _passwordHasher = new PasswordHasher();

    public async void SignIn()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
            {
                User user = await loginService.Login(Username, Password);

                if (user != null)
                {
                    //Tìm xem có lưu User trong dữ liệu cục bộ không 
                    if (Preferences.ContainsKey(nameof(App.user)))
                    {
                        Preferences.Remove(nameof(App.user));
                    }
                    string userDetails = JsonConvert.SerializeObject(user);
                    Preferences.Set(nameof(App.user), userDetails);
                    App.user = user;

                    var memberPageVM = new MemberPageViewModels();
                    var memberPage = new MemberPage(memberPageVM);
                    //await Shell.Current.GoToAsync($"//{nameof(MemberPage)}");
                    await Shell.Current.Navigation.PushAsync(memberPage);

                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Username/Password is incorrect", "Ok");
                    return;
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "All field required", "Ok");
                return;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "Oke");
        }
    }
}