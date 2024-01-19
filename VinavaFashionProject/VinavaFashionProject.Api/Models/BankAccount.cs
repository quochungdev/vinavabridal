using System;
using System.Collections.Generic;

namespace VinavaFashionProject.Api.Models;

public partial class BankAccount
{
    public int Id { get; set; }

    public string? AccountNumber { get; set; }

    public string? AccountHolderName { get; set; }

    public string? BankName { get; set; }
}
