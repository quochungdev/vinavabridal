using System;
using System.Collections.Generic;

namespace VinavaFashionProject.Api.Models;

public partial class ProductAccessory
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? AccessoryId { get; set; }

    public virtual Accessory? Accessory { get; set; }

    public virtual Product? Product { get; set; }
}
