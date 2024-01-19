namespace VinavaFashionProject.Api.DTO
{
    public class Order_OrderDetailDTO
    {
        public int Id { get; set; }

        public string? Color { get; set; }

        public string? Size { get; set; }

        public int? Quantity { get; set; }

        public Order_OrderDetail_ProductDTO? Product { get; set; }
    }
}
