using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinavaFashionProject.Models
{
    public partial class ProductImage
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ImageUrl { get; set; }
        public ImageSource ImageSourceData { get; set; }

        public virtual Product Product { get; set; }
    }
}
