using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinavaFashionProject.Models
{
    public enum OrderStatus
    {
        Placed = 1,
        InformationConfirmed = 2,
        Packaging = 3,
        Shipping = 4,
        Completed = 5
    }
}
