using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.DTO;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;

namespace VinavaFashionProject.ViewModels
{
    public partial class AccountInfoPageViewModel : ObservableObject
    {
        readonly ILoginRepository _loginService = new LoginService();

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _username = App.user.Username;

        [ObservableProperty]
        private string _email = App.user.Email;

        [ObservableProperty]
        private string _fullName = App.user.FullName;

        [ObservableProperty]
        private string _address = App.user.Address;

        [ObservableProperty]
        private DateTime _dateOfBirth = App.user.DateOfBirth;

        [ObservableProperty]
        private string _phoneNumber = App.user.PhoneNumber;

        [ObservableProperty]
        private string _gender = App.user.Gender;

        [ObservableProperty]
        private string _modifiedBy;

        [ObservableProperty]
        private string _modifiedDate;

        [ObservableProperty]
        private string _oldPassword;

        [ObservableProperty]
        private string _newPassword;

        [ObservableProperty]
        private string _enterNewPassword;

        [ICommand]
        private async Task UpdatedUser()
        {
            DateTime currentDateTime = DateTime.Now;

            try
            {
                UserDTO userDTO = new UserDTO
                {
                    Id = App.user.Id,
                    Username = Username,
                    FullName = FullName,
                    Email = Email,
                    PhoneNumber = PhoneNumber,
                    DateOfBirth = DateOfBirth,
                    Gender = Gender,
                    Address = Address,
                    ModifiedBy = Username,
                };
                var updatedUser = await _loginService.UpdateUser(App.user.Id, userDTO);
                if (updatedUser != null)
                {
                    App.user.Username = updatedUser.Username;
                    App.user.DateOfBirth = updatedUser.DateOfBirth;
                    App.user.Gender = updatedUser.Gender;
                    App.user.Email = updatedUser.Email;
                    App.user.FullName = updatedUser.FullName;
                    App.user.PhoneNumber = updatedUser.PhoneNumber;
                    App.user.Address = updatedUser.Address;

                    string userDetails = JsonConvert.SerializeObject(App.user);
                    Preferences.Set(nameof(App.user), userDetails);

                    await Shell.Current.DisplayAlert("Notification", "Update User Successfully", "OK");
                    Console.WriteLine(App.user);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [ICommand]
        public async Task ChangePassword()
        {
            string result = await _loginService.ChangePassword(App.user.Id, OldPassword, NewPassword);
            if (result == null)
            {
                Console.WriteLine("Change Password Failed");
            }
            else
            {
                Console.WriteLine("Change Password Successfully");
            }
        }

    }
}
