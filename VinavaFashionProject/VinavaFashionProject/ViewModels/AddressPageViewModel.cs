using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.DTO;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;

namespace VinavaFashionProject.ViewModels
{
    public partial class AddressPageViewModel : ObservableObject
    {
        readonly IAddressRepository _addressService = new AddressService();

        [ObservableProperty]
        private string _fullName;

        [ObservableProperty]
        private string _phoneNumber;

        [ObservableProperty]
        private string _Ward;

        [ObservableProperty]
        private string _City;

        [ObservableProperty]
        private string _detailAddress;

        [ICommand]
        private async Task AddAddress()
        {
            if (App.user == null)
            {
                await Shell.Current.DisplayAlert("Notification", "Please login", "OK");
                return;
            }
            AddressDTO addressDTO = new AddressDTO
            {
                UserId = App.user.Id,
                FullName = FullName,
                PhoneNumber = PhoneNumber,
                Ward = Ward,
                City = City,
                DetailAddress = DetailAddress,
            };
            bool success = await _addressService.AddAddress(addressDTO);
            if (success)
            {
                await Shell.Current.Navigation.PopAsync();
                //await Shell.Current.DisplayAlert("Notification", "Add Address Successfully", "OK");
            }
        }
    }
}
