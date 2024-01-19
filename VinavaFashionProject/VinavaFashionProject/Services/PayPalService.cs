using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.Services
{
    public class PayPalService : IPayPalRepository
    {
        public async Task<PayPalAccessToken> GetAccessToken(string clientId, string secret)
        {
            var httpClient = new HttpClient();
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.sandbox.paypal.com/v1/oauth2/token");

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                 });

                request.Content = formContent;

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    PayPalAccessToken accessToken = JsonConvert.DeserializeObject<PayPalAccessToken>(responseBody);
                    return accessToken;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<string> CreateInvoiceAndGetInvoiceId(DetailInvoice detail, Amount amount, Invoicer invoicer, IList<PrimaryRecipient> primaryRecipients, IList<ItemInvoice> items, PayPalAccessToken accessToken)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.access_token);

                var invoiceInfo = new InvoiceInfo
                {
                    detail = detail,
                    invoicer = invoicer,
                    primary_recipients = primaryRecipients,
                    items = items,
                    amount = amount
                };

                var json = JsonConvert.SerializeObject(invoiceInfo);

                var body = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://api.sandbox.paypal.com/v2/invoicing/invoices", body);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var invoiceId = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody)["href"].Split("/").Last();
                    return invoiceId;
                }
                else
                {
                    await HandleUnsuccessfulResponse(response);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> SendInvoice(string invoiceId, PayPalAccessToken accessToken)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.access_token);

                var payload = new
                {
                    send_to_invoicer = true
                };

                var jsonPayload = JsonConvert.SerializeObject(payload);
                var body = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"https://api.sandbox.paypal.com/v2/invoicing/invoices/{invoiceId}/send?notify_merchant=false", body);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    await HandleUnsuccessfulResponse(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> SendInvoiceWithReminder(string invoiceId, PayPalAccessToken accessToken)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken.access_token);

                var payload = new
                {
                    subject = "Here your invoice",
                    note = "Please pay before the due date to avoid incurring late payment charges which will be adjusted in the next bill generated.",
                    send_to_invoicer = true
                };

                var jsonPayload = JsonConvert.SerializeObject(payload);
                var body = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"https://api.sandbox.paypal.com/v2/invoicing/invoices/{invoiceId}/remind", body);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    await HandleUnsuccessfulResponse(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private async Task HandleUnsuccessfulResponse(HttpResponseMessage response)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();

            await Shell.Current.DisplayAlert("Error", $"Failed with status code {response.StatusCode}. Message: {errorMessage}", "Ok");
        }
    }
}
