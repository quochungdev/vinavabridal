using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.DTO
{
    public class OrderDTO
    {
        public int UserId { get; set; }

        public int AddressId { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal ShippingFee { get; set; }

        public string Note { get; set; }

        public string PaymentMethod { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string TaxId { get; set; }

        public OrderStatus Status { get; set; }

    }
}
