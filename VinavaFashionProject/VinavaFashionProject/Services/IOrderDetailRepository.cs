using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Api.DTO;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Services
{
    public interface IOrderDetailRepository
    {
        Task<bool> AddOrderDetail(OrderDetailDTO orderDetailDTO);
        Task<List<OrderDetail>> GetOrderDetailsByUserId(int userId);
        List<ProductAttribute> GetProductAttributesByProductIdAndAttributeName(List<OrderDetail> orderDetails, int productId, string attributeName);
        Task<bool> UpdateColor(int id, string newColor);
        Task<bool> UpdateSize(int id, string newSize);
        Task<bool> UpdateQuantity(int id, int newQuantity);
        Task<bool> DeleteOrderDetailAsync(int id);
    }
}
