using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.DTO;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;

namespace VinavaFashionProject.ViewModels
{
    [QueryProperty(nameof(IsPreferential), "isPreferential")]
    [QueryProperty(nameof(TotalAmount), "totalAmount")]
    [QueryProperty(nameof(TotalProduct), "totalProduct")]
    [QueryProperty(nameof(TransportFee), "transportFee")]
    [QueryProperty(nameof(SelectedOrderDetails), "selectedOrderDetails")]

    public partial class PaymentPageViewModel : ObservableObject
    {
        readonly IAddressRepository _addressService = new AddressService();
        readonly IOrderRepository _orderService = new OrderService();
        readonly IPayPalRepository _payPalService = new PayPalService();

        PayPalAccessToken accessToken;
        public string SelectedOrderDetails { get; set; }

        [ObservableProperty]
        private decimal _totalProduct;

        [ObservableProperty]
        private decimal _totalAmount;

        [ObservableProperty]
        private decimal _transportFee;

        [ObservableProperty]
        private string _note;

        [ObservableProperty]
        private string _companyName;

        [ObservableProperty]
        private string _companyAddress;

        [ObservableProperty]
        private string _taxId;

        [ObservableProperty]
        private int _selectedAddressId;

        [ObservableProperty]
        private string _emailBillingInfo = App.user.Email;

        [ObservableProperty]
        private PaymentMethod _selectedPaymentMethod;

        [ObservableProperty]
        private ObservableCollection<Address> _addresses;

        [ObservableProperty]
        private bool _isRead;

        [ObservableProperty]
        private bool _isProcessPayment;

        [ObservableProperty]
        private bool _isPreferential;

        [ObservableProperty]
        private bool _isSelectedPaymentMethod = false;

        public PaymentPageViewModel()
        {
            Addresses = new ObservableCollection<Address>();
        }

        public async Task InitializeAsync()
        {
            if (IsPreferential == true)
            {
                TransportFee = 0;
            }
            await LoadAddressesByUserId();
        }

        [ICommand]
        private async Task LoadAddressesByUserId()
        {
            List<Address> addressesByUserId = await _addressService.GetAddressesByUserId(App.user.Id);
            if (addressesByUserId != null)
            {
                Addresses.Clear();
                foreach (var address in addressesByUserId)
                {
                    Addresses.Add(address);
                }
            }
        }

        [ICommand]
        private void SelectAddress(Address address)
        {
            SelectedAddressId = address.Id;
        }

        [ICommand]
        private async Task ProcessPaymentQRCode()
        {
            IsProcessPayment = true;

            if (SelectedAddressId == -1)
            {
                await Shell.Current.DisplayAlert("Notification", "Please select a specific Address", "Ok");
                IsProcessPayment = false;
                return;
            }

            decimal exchangeRate = await _orderService.GetExchangeRate("USD");
            decimal totalAmountVND = TotalAmount * exchangeRate;

            OrderDTO newOrder = new OrderDTO
            {
                UserId = App.user.Id,
                AddressId = SelectedAddressId,
                TotalAmount = TotalAmount,
                ShippingFee = TransportFee,
                Note = Note,
                PaymentMethod = SelectedPaymentMethod.ToString(),
                CompanyName = CompanyName,
                CompanyAddress = CompanyAddress,
                TaxId = TaxId,
                Status = OrderStatus.Placed
            };

            int orderId = await _orderService.CreateOrder(newOrder);
            if (orderId > 0)
            {
                await Shell.Current.GoToAsync($"{nameof(PaymentSuccessPage)}?totalAmount={totalAmountVND}&orderId={orderId}");
            }
            IsProcessPayment = false;
        }

        [ICommand]
        private async Task ProcessPaymentPayPal()
        {
            IsProcessPayment = true;
            string clientId = "AWrnxDyyGqzQBxw4cR2xcq_JSppv0-I6UQsahBhIXlm8MTE1cSdsJExf-tk0_jBEsZVKfr7l10mVejzX";
            string secretKey = "EFv0LzMapFP9I_xcj69XX4a3IqBtBHcGyLIUCXqlxaU_xGgF6vitwkfPbk-NtEaXKzoYx8QMSIpZp5du";
            accessToken = await _payPalService.GetAccessToken(clientId, secretKey);

            var temporarySelectedOrderDetails = JsonConvert.DeserializeObject<List<dynamic>>(SelectedOrderDetails);
            var orderDetails = temporarySelectedOrderDetails.Select(detail => new OrderDetail
            {
                Quantity = detail.OdQuantity,
                Product = new Product
                {
                    Price = detail.OdPrice,
                    Name = detail.OdName,
                }
            }).ToList();

            List<ItemInvoice> items = new List<ItemInvoice>();
            foreach (var od in orderDetails)
            {
                ItemInvoice item = new ItemInvoice
                {
                    name = od.Product.Name,
                    quantity = od.Quantity.ToString(),
                    unit_amount = new UnitAmount { currency_code = "USD", value = od.Product.Price.ToString() },
                    unit_of_measure = "QUANTITY"
                };
                items.Add(item);
            }

            var detail = new DetailInvoice
            {
                currency_code = "USD",
                note = "Note",
                term = "No refunds after 30 days."
            };

            var invoicer = new Invoicer
            {
                name = new NameInvoice { given_name = "Quoc Hung", surname = "Bussiness" },
                address = new AddressInvoice { address_line_1 = "422 NVC", address_line_2 = "Binh Thanh", admin_area_1 = "Ho Chi Minh", postal_code = "70000", country_code = "VN" },
                email_address = "quochungbusiness@gmail.com",
                phones = new[]
                {
                    new Phone
                    {
                        country_code = "001",
                        national_number = "4085551234",
                        phone_type = "MOBILE"
                    }
                }
            };

            var primaryRecipient = new[]
            {
               new PrimaryRecipient
               {
                  billing_info = new BillingInfo
                    {
                        name = new NameInvoice { given_name = App.user.FullName, surname = "Customer" },
                        email_address = EmailBillingInfo
                    }
               }
            };

            var amount = new Amount
            {
                breakdown = new Breakdown
                {
                    shipping = new Shipping
                    {
                        amount = new SubAmount { currency_code = "USD", value = TransportFee.ToString() }
                    }
                }
            };

            var invoiceId = await _payPalService.CreateInvoiceAndGetInvoiceId(detail, amount, invoicer, primaryRecipient, items, accessToken);
            bool isSend = await _payPalService.SendInvoice(invoiceId, accessToken);
            if (isSend)
            {
                bool isRemind = await _payPalService.SendInvoiceWithReminder(invoiceId, accessToken);
                if (isRemind)
                {
                    OrderDTO newOrder = new OrderDTO
                    {
                        UserId = App.user.Id,
                        AddressId = SelectedAddressId,
                        TotalAmount = TotalAmount,
                        ShippingFee = TransportFee,
                        Note = Note,
                        PaymentMethod = SelectedPaymentMethod.ToString(),
                        CompanyName = CompanyName,
                        CompanyAddress = CompanyAddress,
                        TaxId = TaxId,
                        Status = OrderStatus.Placed
                    };
                    int orderId = await _orderService.CreateOrder(newOrder);
                    if (orderId > 0)
                    {
                        IsProcessPayment = false;
                        await Shell.Current.GoToAsync($"{nameof(PaymentPayPalSuccessPage)}?emailBillingInfo={EmailBillingInfo}");
                    }
                    else
                        IsProcessPayment = false;
                }
                return;
            }
            IsProcessPayment = false;
            return;
        }
    }
}
