using System;
using System.Collections.Generic;

namespace VinavaFashionProject.Api.Models;

public partial class Accessory
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<ProductAccessory> ProductAccessories { get; set; } = new List<ProductAccessory>();
}
