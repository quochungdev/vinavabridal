using CommunityToolkit.Maui.Views;
using VinavaFashionProject.Models;
using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class PaymentPage : ContentPage
{
    private PaymentPageViewModel _viewmodel;

    private bool IsPaymentByQRCodeChecked;
    private bool IsPaymentByPayPalChecked;

    public PaymentPage()
    {
        InitializeComponent();
        _viewmodel = new PaymentPageViewModel();
        BindingContext = _viewmodel;
    }

    private void OnPopupClicked(object sender, EventArgs e)
    {
        if (BindingContext is PaymentPageViewModel vm)
        {
            var popupViewModel = new PopupPayPalInfoViewModel();
            var popup = new PopupPayPalInfo(popupViewModel);
            this.ShowPopup(popup);
        }
    }

    private async void OnTappedAddAddress(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AddAddressPage));
    }

    private void OnAddressCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            if (radioButton.BindingContext is Address address)
            {
                _viewmodel.SelectAddressCommand.Execute(address);
            }
        }
    }

    private void OnPaymentByQRCodeCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _viewmodel.SelectedPaymentMethod = PaymentMethod.QRCode;
            _viewmodel.IsRead = true;
            IsPaymentByQRCodeChecked = true;
            IsPaymentByPayPalChecked = false;

            _viewmodel.IsSelectedPaymentMethod = true;
        }
    }

    private void OnPaymentByPayPalCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _viewmodel.SelectedPaymentMethod = PaymentMethod.PayPal;
            _viewmodel.IsRead = false;
            IsPaymentByPayPalChecked = true;
            IsPaymentByQRCodeChecked = false;

            _viewmodel.IsSelectedPaymentMethod = true;
        }
    }

    private async void GoToPaymentSuccess(object sender, EventArgs e)
    {
        if (_viewmodel.IsSelectedPaymentMethod == false)
        {
            await Shell.Current.DisplayAlert("Notification", "Please select Payment method", "Ok");
            _viewmodel.IsProcessPayment = false;
            return;
        }

        if (IsPaymentByPayPalChecked)
        {
            await _viewmodel.ProcessPaymentPayPalCommand.ExecuteAsync(null);
        }
        if (IsPaymentByQRCodeChecked)
        {
            await _viewmodel.ProcessPaymentQRCodeCommand.ExecuteAsync(null);
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewmodel.InitializeAsync();
    }
}