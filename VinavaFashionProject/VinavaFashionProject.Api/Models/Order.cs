using System;
using System.Collections.Generic;

namespace VinavaFashionProject.Api.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? AddressId { get; set; }

    public int? OfferId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? TotalAmount { get; set; }

    public decimal? ShippingFee { get; set; }

    public string? Note { get; set; }

    public string? PaymentMethod { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyAddress { get; set; }

    public string? TaxId { get; set; }

    public int? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public Guid? RowId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual Offer? Offer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User? User { get; set; }
}
