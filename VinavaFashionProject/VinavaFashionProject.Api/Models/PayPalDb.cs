using System;
using System.Collections.Generic;

namespace VinavaFashionProject.Api.Models;

public partial class PayPalDb
{
    public int Id { get; set; }

    public string? PayPalAccount { get; set; }

    public string? Instruct { get; set; }

    public string? Note { get; set; }
}
