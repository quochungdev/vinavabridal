using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinavaFashionProject.Models
{
    public class DetailInvoice
    {
        public string currency_code { get; set; }
        public string note { get; set; }
        public string term { get; set; }
    }

    public class NameInvoice
    {
        public string given_name { get; set; }
        public string surname { get; set; }
    }

    public class AddressInvoice
    {
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string admin_area_1 { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
    }

    public class Phone
    {
        public string country_code { get; set; }
        public string national_number { get; set; }
        public string phone_type { get; set; }
    }

    public class Invoicer
    {
        public NameInvoice name { get; set; }
        public AddressInvoice address { get; set; }
        public string email_address { get; set; }
        public IList<Phone> phones { get; set; }
    }

    public class BillingInfo
    {
        public NameInvoice name { get; set; }
        public string email_address { get; set; }
    }

    public class PrimaryRecipient
    {
        public BillingInfo billing_info { get; set; }
    }

    public class UnitAmount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    //public class Shipping
    //{
    //    public UnitAmount amount;
    //}

    //public class Breakdown
    //{
    //    public Shipping shipping { get; set; }
    //}

    //public class Amount
    //{
    //    public string currency_code { get; set; }
    //    public string value { get; set; }
    //    public Breakdown breakdown { get; set; }
    //}

    public class SubAmount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class Shipping
    {
        public SubAmount amount { get; set; }
    }

    public class Breakdown
    {
        public Shipping shipping { get; set; }
    }

    public class Amount
    {
        public Breakdown breakdown { get; set; }
    }

    public class ItemInvoice
    {
        public string name { get; set; }
        public string quantity { get; set; }
        public UnitAmount unit_amount { get; set; }
        public string unit_of_measure { get; set; }
    }

    public class InvoiceInfo
    {
        public DetailInvoice detail { get; set; }
        public Invoicer invoicer { get; set; }
        public IList<PrimaryRecipient> primary_recipients { get; set; }
        public IList<ItemInvoice> items { get; set; }
        public Amount amount { get; set; }
    }
}
