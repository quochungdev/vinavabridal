using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;

namespace VinavaFashionProject.ViewModels
{
    [QueryProperty(nameof(OrderId), nameof(OrderId))]
    public partial class OrderDetailPageViewModel : ObservableObject
    {
        readonly IOrderRepository _orderService = new OrderService();

        [ObservableProperty]
        private int _orderId;

        [ObservableProperty]
        private Order _orderVM;

        [ObservableProperty]
        private ObservableCollection<Order> _orderDetails;

        [ObservableProperty]
        private string _qrCodeData;

        [ObservableProperty]
        private byte[] _qrCodeImageBytes;

        [ObservableProperty]
        private ImageSource _qrCodeImage;

        [ObservableProperty]
        private BankAccount _bank;

        public OrderDetailPageViewModel()
        {
        }
        public async Task InitializeAsync()
        {
            await LoadOrder();
        }
        private async Task LoadOrder()
        {
            var order = await _orderService.GetOrdersById(OrderId);
            foreach (var o in order.OrderDetails)
            {
                UpdateImageSource(o);
            }

            OrderVM = order;
            CreateQRCode();
        }

        private void UpdateImageSource(OrderDetail orderDetail)
        {
            if (!string.IsNullOrEmpty(orderDetail.Product.ImageUrl))
            {
                byte[] imageBytes = Convert.FromBase64String(orderDetail.Product.ImageUrl);
                ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));

                orderDetail.Product.ImageSourceData = imageSource;
            }
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
            decimal exchangeRate = await _orderService.GetExchangeRate("USD");
            decimal totalAmountVND = OrderVM.TotalAmount * exchangeRate;

            BankAccount ba = await _orderService.GetBankAccount();
            Bank = ba;
            VietQR myVietQR = new VietQR();
            myVietQR.SetTransactionAmount(totalAmountVND).SetBeneficiaryOrganization("970407", "19050224869019").SetAdditionalDataFieldTemplate($"THANH TOAN DON HANG {OrderId} TAI VINAVA");
            GenerateVietQRCode(myVietQR);
        }
    }
}
