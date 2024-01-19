using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.DTO;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Services
{
    public interface IOrderRepository
    {
        Task<int> CreateOrder(OrderDTO order);
        Task<List<Order>> GetOrdersByUserId(int userId);
        Task<Order> GetOrdersById(int orderId);
        Task<ApiResponse> GenerateQRCode(decimal enterAmount, string enterInfo);
        Task<decimal> GetExchangeRate(string currency);
        Task<PayPalDb> GetPayPalDB();
        Task<BankAccount> GetBankAccount();
    }
}
