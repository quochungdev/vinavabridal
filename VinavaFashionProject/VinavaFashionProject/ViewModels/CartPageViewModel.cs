using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Api.DTO;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;

namespace VinavaFashionProject.ViewModels
{
    public partial class CartPageViewModel : ObservableObject
    {
        readonly IOrderDetailRepository _orderDetailService = new OrderDetailService();

        [ObservableProperty]
        private ObservableCollection<OrderDetail> _orderDetails;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private bool _isCartEmpty;

        [ObservableProperty]
        private decimal _totalProduct;

        [ObservableProperty]
        private decimal _totalAmount;

        [ObservableProperty]
        private decimal _transportFee = 0;

        [ObservableProperty]
        private bool _isPreferential = false;

        [ObservableProperty]
        private bool _isSelectedMethodFee = false;

        public CartPageViewModel()
        {
            //_orderDetails = new List<OrderDetail>();
            //LoadOrderDetailsByUserId();
        }
        public async Task InitializeAsync()
        {
            IsBusy = true;

            _orderDetails = new ObservableCollection<OrderDetail>();
            await LoadOrderDetailsByUserId();

            IsCartEmpty = OrderDetails == null || OrderDetails.Count == 0;

            IsBusy = false;
        }

        [ICommand]
        async Task GoToPayment()
        {
            if(IsSelectedMethodFee == false)
            {
                await Shell.Current.DisplayAlert("Notification", "Please select shipping location", "Ok");
                return;
            }
            foreach(var od in OrderDetails)
            {
                if (od.Quantity >= 2 || OrderDetails.Count >= 2)
                {
                    IsPreferential = true;
                }
                else 
                    IsPreferential = false;
            }
            if(IsPreferential == true)
            {
                TotalAmount = TotalAmount - TransportFee;
            }

            var selectedOrderDetails = OrderDetails.Select(detail => new
            {
                OdPrice = detail.Product.Price,
                OdName = detail.Product.Name,
                OdQuantity = detail.Quantity
            }).ToList();
            string selectedOrderDetailsJson = JsonConvert.SerializeObject(selectedOrderDetails);
            await Shell.Current.GoToAsync($"{nameof(PaymentPage)}?totalAmount={TotalAmount}&isPreferential={IsPreferential}&totalProduct={TotalProduct}&transportFee={TransportFee}&selectedOrderDetails={WebUtility.UrlEncode(selectedOrderDetailsJson)}");
        }

        [ICommand]
        async Task GoToProductPage()
        {
            await Shell.Current.Navigation.PopToRootAsync();
            await Shell.Current.GoToAsync("//ProductPage");
        }

        [ICommand]
        private async Task LoadOrderDetailsByUserId()
        {
            //IsBusy = true;
            if (App.user != null)
            {
                List<OrderDetail> orderDetailByUID = await _orderDetailService.GetOrderDetailsByUserId(App.user.Id);

                if (orderDetailByUID == null || orderDetailByUID.Count == 0)
                {
                    return;
                }

                OrderDetails.Clear();

                foreach (var od in orderDetailByUID)
                {
                    UpdateImageSource(od);
                    TotalProduct += od.Total;

                    var selectedColorAttribute = od.ColorAttributes.FirstOrDefault(attr => attr.Attribute.AttributeValue == od.Color);
                    var selectedSizeAttribute = od.SizeAttributes.FirstOrDefault(attr => attr.Attribute.AttributeValue == od.Size);

                    od.SelectedColor = selectedColorAttribute;
                    od.SelectedSize = selectedSizeAttribute;
                }
                TotalAmount = TotalProduct + TransportFee;
                OrderDetails = new ObservableCollection<OrderDetail>(orderDetailByUID);
            }
            else return;
        }

        [ICommand]
        private async void ChangeColor(OrderDetail orderDetail)
        {
            await _orderDetailService.UpdateColor(orderDetail.Id, orderDetail.Color);
        }

        [ICommand]
        private async void ChangeSize(OrderDetail orderDetail)
        {
            await _orderDetailService.UpdateSize(orderDetail.Id, orderDetail.Size);
        }

        [ICommand]
        private async void IncreaseQuantity(OrderDetail orderDetail)
        {
            decimal previousTotal = orderDetail.Total;
            orderDetail.Quantity++;

            orderDetail.Total = orderDetail.Price * orderDetail.Quantity;

            UpdateTotalProduct(orderDetail, previousTotal);
            UpdateTotalAmount();
            await _orderDetailService.UpdateQuantity(orderDetail.Id, orderDetail.Quantity);
        }

        [ICommand]
        private async void DecreaseQuantity(OrderDetail orderDetail)
        {
            if (orderDetail.Quantity > 1)
            {
                decimal previousTotal = orderDetail.Total;
                orderDetail.Quantity--;

                orderDetail.Total = orderDetail.Price * orderDetail.Quantity;

                UpdateTotalProduct(orderDetail, previousTotal);
                UpdateTotalAmount();
                await _orderDetailService.UpdateQuantity(orderDetail.Id, orderDetail.Quantity);
            }
        }

        [ICommand]
        private async void DeleteOrderDetail(OrderDetail orderDetail)
        {
            var answer = await Shell.Current.DisplayAlert("Notification", $"\nDo you want to delete the product?\n\n{orderDetail.Product.Name}\n\n", "Yes", "Cancel");
            if (answer)
            {
                decimal previousTotal = orderDetail.Total;

                await _orderDetailService.DeleteOrderDetailAsync(orderDetail.Id);
                OrderDetails.Remove(orderDetail);
                TotalProduct += 0 - previousTotal;

                UpdateTotalAmount();

                if (OrderDetails.Count == 0)
                {
                    IsCartEmpty = true;
                }
                Console.WriteLine(IsCartEmpty);
            }
            else
            {
                return;
            }
        }
        private void UpdateTotalProduct(OrderDetail orderDetail, decimal previousTotal)
        {
            TotalProduct += orderDetail.Total - previousTotal;
        }

        private void UpdateTotalAmount()
        {
            TotalAmount = TotalProduct + TransportFee;
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
    }
}
