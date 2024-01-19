using VinavaFashionProject.Api.Models;

namespace VinavaFashionProject.Api.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        public int? AddressId { get; set; }

        //public int? OfferId { get; set; }

        public DateTime? OrderDate { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? ShippingFee { get; set; }

        public string? Note { get; set; }

        public string? PaymentMethod { get; set; }

        public int? Status { get; set; }

        public Order_AddressDTO? Address { get; set; }
        //public virtual Offer? Offer { get; set; }

        public List<Order_OrderDetailDTO>? OrderDetails { get; set; }
    }
}
