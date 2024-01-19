using VinavaFashionProject.Api.DTO;

namespace VinavaFashionProject.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; }

    public string FullName { get; set; }

    public string Address { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; }

    public string PhoneNumber { get; set; }

    public string Otp { get; set; }
    //public List<OrderDetailDTO> OrderDetails { get; set; }

    //public string ModifiedBy { get; set; }

    //public Guid RowId { get; set; }

    //public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    //public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();

    //public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}