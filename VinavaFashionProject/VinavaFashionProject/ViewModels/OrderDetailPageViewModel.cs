using CommunityToolkit.Mvvm.ComponentModel;
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
