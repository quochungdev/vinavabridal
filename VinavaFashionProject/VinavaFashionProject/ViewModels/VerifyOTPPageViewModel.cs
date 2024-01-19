using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;

namespace VinavaFashionProject.ViewModels;
[QueryProperty(nameof(UserId), "userId")]
[QueryProperty(nameof(Email), "email")]

public partial class VerifyOTPPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string[] _otp = new string[6];

    [ObservableProperty]
    public int _userId;

    [ObservableProperty]
    public string _email;

    [ObservableProperty]
    public string _newPassword;

    [ObservableProperty]
    public string _enterNewPassword;

    readonly ILoginRepository _loginService = new LoginService();

    public VerifyOTPPageViewModel()
    {
        _loginService = new LoginService();
    }

    [ICommand]
    public async Task Verify()
    {
        string enteredOTP = string.Join("", Otp);
        if (UserId != null)
        {
            string result = await _loginService.VerifyOTP(UserId, enteredOTP);
            if (result == "Verify OTP Successfully")
            {
                await Shell.Current.GoToAsync(nameof(ResetPasswordPage));
            }
        }
    }

    [ICommand]
    public async Task ResetPassword()
    {
        if (NewPassword != EnterNewPassword)
        {
            await Shell.Current.DisplayAlert("Error", "Passwords do not match", "OK");
            return;
        }
        else
        {
            string result = await _loginService.ResetPassword(UserId, NewPassword);
            if (result == "Reset password Successfully")
            {
                await Shell.Current.Navigation.PopToRootAsync();
            }
            else return;
        }


    }
}