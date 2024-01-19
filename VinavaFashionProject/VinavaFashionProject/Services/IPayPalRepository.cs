using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Services
{
    public interface IPayPalRepository
    {
        Task<PayPalAccessToken> GetAccessToken(string clientId, string secret);
        Task<string> CreateInvoiceAndGetInvoiceId(DetailInvoice detail, Amount amount, Invoicer invoicer, IList<PrimaryRecipient> primaryRecipients, IList<ItemInvoice> items, PayPalAccessToken accessToken);
        Task<bool> SendInvoice(string invoiceId, PayPalAccessToken accessToken);
        Task<bool> SendInvoiceWithReminder(string invoiceId, PayPalAccessToken accessToken);
    }
}
