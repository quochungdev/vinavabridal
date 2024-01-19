using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinavaFashionProject.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAccessory { get; set; }
    }

}
