using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.DTO;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Services
{
    public interface IAddressRepository
    {
        Task<bool> AddAddress(AddressDTO addressDTO);
        Task<List<Address>> GetAddressesByUserId(int userId);
    }
}
