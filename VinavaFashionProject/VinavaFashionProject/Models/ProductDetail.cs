namespace VinavaFashionProject.Models
{
    public class ProductDetail
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string StorageInstructions { get; set; }

        public string SizeImage { get; set; }

        public string Idstring { get; set; }
        public Category Category { get; set; }
        public List<ProductAttribute> ProductAttributes { get; set; }
        //public List<ProductImage> ProductImages { get; set; }

    }
}
