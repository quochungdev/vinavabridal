using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinavaFashionProject.DTO
{
    public class AddressDTO
    {
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Ward { get; set; }

        public string City { get; set; }

        public string DetailAddress { get; set; }
    }
}
