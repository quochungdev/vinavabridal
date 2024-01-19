using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;

namespace VinavaFashionProject.ViewModels;

public partial class RegisterPageViewModel : ObservableObject
{
    readonly ILoginRepository _loginService = new LoginService();
    readonly IPasswordHasher _passwordHasher = new PasswordHasher();

    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private string _fullName;

    [ObservableProperty]
    private DateTime _dateOfBirth;

    [ObservableProperty]
    private string _phoneNumber;

    [ObservableProperty]
    private string _createdBy;

    public RegisterPageViewModel()
    {
        DateOfBirth = DateTime.Today;
    }

    [ICommand]
    private async Task Register()
    {

        try
        {
            if (!string.IsNullOrWhiteSpace(Username) &&
               !string.IsNullOrWhiteSpace(Password) &&
               !string.IsNullOrWhiteSpace(Email) &&
               !string.IsNullOrWhiteSpace(FullName) &&
               !string.IsNullOrWhiteSpace(PhoneNumber))
            {
                if (!IsValidEmail(Email))
                {
                    await Shell.Current.DisplayAlert("Error", "Email address is not valid", "Ok");
                    return;
                }
                var passwordHash = _passwordHasher.Hash(Password);
                User user = new User
                {
                    Username = Username,
                    Password = passwordHash,
                    FullName = FullName,
                    Email = Email,
                    DateOfBirth = DateOfBirth,
                    PhoneNumber = PhoneNumber,
                };

                bool result = await _loginService.CreateUser(user);
                if (result == true)
                {
                    await Shell.Current.DisplayAlert("Allert", "Successfully Register", "Ok");
                    await Shell.Current.Navigation.PopAsync();
                    await Shell.Current.GoToAsync(nameof(LoginPage));
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Please enter complete information", "Ok");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

}