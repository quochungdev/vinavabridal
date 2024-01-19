namespace VinavaFashionProject.Api.DTO
{
    public class OrderDetailByUserDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        public int? ProductId { get; set; }

        public string? Color { get; set; }

        public string? Size { get; set; }

        public int? Quantity { get; set; }

        public decimal? Price { get; set; }

        public decimal? Total { get; set; }
        public ProductDTO? Product { get; set; }

    }
}
