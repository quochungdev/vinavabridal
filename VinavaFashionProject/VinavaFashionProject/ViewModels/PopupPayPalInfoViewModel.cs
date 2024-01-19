using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;

namespace VinavaFashionProject.ViewModels
{
    public partial class PopupPayPalInfoViewModel : ObservableObject
    {
        readonly IOrderRepository _orderService = new OrderService();

        [ObservableProperty]
        private PayPalDb _paypalInfo;

        public PopupPayPalInfoViewModel() {
            InitializeAsync();
        }
        public async Task InitializeAsync()
        {
            await LoadPayPayInfo();
        }

        private async Task LoadPayPayInfo()
        {
            PayPalDb ppInfo = await _orderService.GetPayPalDB();
            PaypalInfo = ppInfo;
        }
    }
}
