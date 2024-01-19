using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth.Requests;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;

namespace VinavaFashionProject.ViewModels;

public partial class ForgotPasswordViewModel : ObservableObject
{
    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private bool _isProcessPayment;

    readonly ILoginRepository _loginService = new LoginService();

    public ForgotPasswordViewModel()
    {
        _loginService = new LoginService();
    }

    [ICommand]
    public async Task ResetPassword()
    {
        IsProcessPayment = true;

        int? userId = await _loginService.GetUserIdByEmail(Email);
        if (userId != null)
        {
            string result = await _loginService.SendOTP(userId.Value);
            if (result == "OTP sent successfully")
            {
                await Shell.Current.DisplayAlert("Notification", "The OTP code has been sent to your email", "OK");
                await Shell.Current.GoToAsync($"{nameof(VerifyOTPPage)}?userId={userId.Value}&email={Email}");
                IsProcessPayment = false;
            }
            else
            {
                IsProcessPayment = false;
                await Shell.Current.DisplayAlert("Error", "This email does not exist on the system", "OK");
                return;
            }
        }
        else
        {
            IsProcessPayment = false;
            await Shell.Current.DisplayAlert("Error", "This email does not exist on the system", "OK");
            return;
        }
    }
}