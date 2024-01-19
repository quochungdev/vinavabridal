using System;
using System.Collections.Generic;
using VinavaFashionProject.Api.Models;

namespace VinavaFashionProject.Api.DTO;

public partial class FavoriteProductDTO
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public DateTime? FavoriteDate { get; set; }
    public FavouriteProduct_ProductDTO? Product { get; set; }

}
