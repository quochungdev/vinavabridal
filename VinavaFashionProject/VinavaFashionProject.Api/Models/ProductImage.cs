﻿using System;
using System.Collections.Generic;

namespace VinavaFashionProject.Api.Models;

public partial class ProductImage
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Product? Product { get; set; }
}
