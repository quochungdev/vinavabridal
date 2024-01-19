using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VinavaFashionProject.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int AddressId { get; set; }
        //public int OfferId { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal ShippingFee { get; set; }

        public string Note { get; set; }

        public string PaymentMethod { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string TaxId { get; set; }

        public OrderStatus Status { get; set; }

        public Address Address { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        //public User User { get; set; }

    }
}
