using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinavaFashionProject.Models
{
    public class FavouriteProductDTO
    {
        public int UserId { get; set; }

        public int ProductId { get; set; }

        public DateTime FavoriteDate { get; set; }
    }
}
