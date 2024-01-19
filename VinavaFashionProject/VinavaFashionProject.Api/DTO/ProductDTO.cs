using System.Text.Json.Serialization;
using VinavaFashionProject.Api.Models;

namespace VinavaFashionProject.Api.DTO
{
    public partial class ProductDTO
    {
        public int Id { get; set; }

        public int? CategoryId { get; set; }

        public string? Name { get; set; }

        //public string? Description { get; set; }

        public decimal? Price { get; set; }

        //public int? Quantity { get; set; }

        public string? ImageUrl { get; set; }

        //public string? StorageInstructions { get; set; }

        //public string? SizeImage { get; set; }

        public bool? IsNew { get; set; }

        [JsonIgnore]
        public decimal? SaleDiscount { get; set; }
        public string? SaleDiscountString
        {
            get
            {
                return SaleDiscount.HasValue ? SaleDiscount.Value.ToString() : null;
            }
        }

        public string? Idstring { get; set; }
        public CategoryDTO? Category { get; set; }
        public List<ProductAttributeDTO>? ProductAttributes { get; set; }
    }
}
