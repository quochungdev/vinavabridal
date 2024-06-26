﻿using VinavaFashionProject.Api.Models;

namespace VinavaFashionProject.Api.DTO
{
    public class AddressDTO
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Ward { get; set; }

        public string? City { get; set; }

        public string? DetailAddress { get; set; }

    }
}
