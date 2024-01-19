using System;
using System.Collections.Generic;

namespace VinavaFashionProject.Api.Models;

public partial class ExchangeRate
{
    public int Id { get; set; }

    public string? Currency { get; set; }

    public decimal? Rate { get; set; }

    public DateTime? Date { get; set; }
}
