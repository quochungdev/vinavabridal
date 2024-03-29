﻿namespace VinavaFashionProject.Api.DTO
{
    public class ProductDetailDTO
    {
        public int Id { get; set; }

        public int? CategoryId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        //public string? ImageUrl { get; set; }

        public string? StorageInstructions { get; set; }

        public string? SizeImage { get; set; }

        public string? Idstring { get; set; }
        public CategoryDTO Category { get; set; }
        public List<ProductAttributeDTO> ProductAttributes { get; set; }
        //public List<ProductImageDTO> ProductImages { get; set; }

    }
}
