using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;

namespace VinavaFashionProject.ViewModels
{
    public partial class OrderPageViewModel : ObservableObject
    {
        readonly IOrderRepository _orderService = new OrderService();

        [ObservableProperty]
        private ObservableCollection<Order> _orders;
        public OrderPageViewModel()
        {
        }
        public async Task InitializeAsync()
        {
            Orders = new ObservableCollection<Order>();

            await LoadOrders();
        }

        [ICommand]
        async Task GoToOrderDetailPage(Order order)
        {
            await Shell.Current.GoToAsync($"{nameof(OrderDetailPage)}?OrderId={order.Id}");
        }

        [ICommand]
        private async Task LoadOrders()
        {
            if (App.user != null)
            {
                List<Order> ordersByUserID = await _orderService.GetOrdersByUserId(App.user.Id);
                if (ordersByUserID == null || ordersByUserID.Count == 0)
                {
                    return;
                }

                Orders.Clear();

                foreach (var o in ordersByUserID)
                {
                    Orders.Add(o);
                }
            }
        }
    }
}
