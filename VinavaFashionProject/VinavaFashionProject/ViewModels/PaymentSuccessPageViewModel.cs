using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;
using ZXing;
using ZXing.QrCode;

namespace VinavaFashionProject.ViewModels
{
    [QueryProperty(nameof(TotalAmount), "totalAmount")]
    [QueryProperty(nameof(EmailBillingInfo), "emailBillingInfo")]
    [QueryProperty(nameof(OrderId), "orderId")]
    public partial class PaymentSuccessPageViewModel : ObservableObject
    {
        readonly IOrderRepository _orderService = new OrderService();

        [ObservableProperty]
        private string _emailBillingInfo;

        [ObservableProperty]
        private ImageSource _qrCodeImage;

        [ObservableProperty]
        private decimal _totalAmount;

        [ObservableProperty]
        private string _qrCodeData;

        [ObservableProperty]
        private byte[] _qrCodeImageBytes;

        [ObservableProperty]
        private int _orderId;

        [ObservableProperty]
        private BankAccount _bank;

        private ImageSource Base64ToImage(string base64String)
        {
            if (!string.IsNullOrEmpty(base64String))
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                return imageSource;
            }
            else
                return null;
        }

        [ICommand]
        private void GenerateQRCode(string qrCodeData)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeObj = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeObj);
            byte[] qrCodeBytes = qrCode.GetGraphic(20);
            QrCodeImage = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
        }

        private void GenerateVietQRCode(VietQR vietQR)
        {
            string qrCodeData = $"{vietQR.payloadFormatIndicator}{vietQR.consumerAccountInformation}{vietQR.transactionCurrency}{vietQR.transactionAmount}{vietQR.countryCode}{vietQR.additionalDataFieldTemplate}";

            string crc = vietQR.CreatePaymentCRC(qrCodeData);

            vietQR.crc = vietQR.QRCRC + crc;

            string qrCodeCRC = qrCodeData + vietQR.crc;

            Console.WriteLine(vietQR.consumerAccountInformation);
            GenerateQRCode(qrCodeCRC);
        }

        [ICommand]
        private async void CreateQRCode()
        {
            BankAccount ba = await _orderService.GetBankAccount();
            Bank = ba;
            VietQR myVietQR = new VietQR();
            myVietQR.SetTransactionAmount(TotalAmount).SetBeneficiaryOrganization("970407", "19050224869019").SetAdditionalDataFieldTemplate("THANH TOAN DON HANG VINAVA");

            GenerateVietQRCode(myVietQR);
        }


        [ICommand]
        async Task GoToOrderPage()
        {
            await Shell.Current.Navigation.PopToRootAsync();
            await Shell.Current.GoToAsync("//OrderPage");
        }
    }
}
