﻿namespace VinavaFashionProject.Api.DTO
{
    public class OrderDetailDTO
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Color { get; set; }

        public string Size { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Total { get; set; }
    }
}
