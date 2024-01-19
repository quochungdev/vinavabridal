namespace VinavaFashionProject.Api.DTO
{
    public class UserDTO
    {
        public int? Id { get; set; }
        public string? Username { get; set; } = null!;

        public string? Password { get; set; } = null!;

        public string? Email { get; set; }

        public string? FullName { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Otp { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public List<OrderDetailDTO>? OrderDetails { get; set; }
    }
}
