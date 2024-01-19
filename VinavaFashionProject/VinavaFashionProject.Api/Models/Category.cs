using System;
using System.Collections.Generic;

namespace VinavaFashionProject.Api.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool? IsAccessory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
