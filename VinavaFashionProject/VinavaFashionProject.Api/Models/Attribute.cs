using System;
using System.Collections.Generic;

namespace VinavaFashionProject.Api.Models;

public partial class Attribute
{
    public int Id { get; set; }

    public string? AttributeName { get; set; }

    public string? AttributeValue { get; set; }

    public virtual ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();
}
