using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinavaFashionProject.Models
{
    public class Address
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Ward { get; set; }

        public string City { get; set; }

        public string DetailAddress { get; set; }

        public string AddressCombine => $"{DetailAddress} - {Ward} - {City}";
        //public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        //public virtual User? User { get; set; }
    }
}
