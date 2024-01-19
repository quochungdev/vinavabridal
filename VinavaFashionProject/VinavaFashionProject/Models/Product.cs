using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinavaFashionProject.Models
{
    public class Product
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
        public bool IsNew { get; set; }
        public string SaleDiscountString { get; set; }

        public string ImageUrl { get; set; }

        public ImageSource ImageSourceData { get; set; }

        public string StorageInstructions { get; set; }

        public string SizeImage { get; set; }

        public string Idstring { get; set; }

        public virtual Category Category { get; set; }
        public List<ProductAttribute> ProductAttributes { get; set; }

    }
}
