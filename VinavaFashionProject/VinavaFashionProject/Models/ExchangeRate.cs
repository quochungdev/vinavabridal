using System;
using System.Collections.Generic;

namespace VinavaFashionProject.Models;

public class ExchangeRate
{
    public int Id { get; set; }

    public string Currency { get; set; }

    public decimal Rate { get; set; }

    public DateTime Date { get; set; }
}
