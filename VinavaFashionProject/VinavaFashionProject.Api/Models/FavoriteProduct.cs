using System;
using System.Collections.Generic;

namespace VinavaFashionProject.Api.Models;

public partial class FavoriteProduct
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public DateTime? FavoriteDate { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public Guid? RowId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
