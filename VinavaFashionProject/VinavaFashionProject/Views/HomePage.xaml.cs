using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;
using VinavaFashionProject.Models;
using VinavaFashionProject.ViewModels;
using System;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Sentry;

namespace VinavaFashionProject.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private HomePageViewModels _viewmodel;

        public HomePage()
        {
            InitializeComponent();
            _viewmodel = new HomePageViewModels();
            BindingContext = _viewmodel;

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await InitializeAsync();
            //await _viewmodel.InitializeCommand.ExecuteAsync(null);
        }
        private async Task InitializeAsync()
        {
            try
            {
                //await _viewmodel.InitializeCommand.ExecuteAsync(null);
                await Task.Run(() => _viewmodel.InitializeCommand.Execute(null));
                System.Diagnostics.Debug.WriteLine("InitializeAsync completed successfully");
                SentrySdk.CaptureMessage("Wrong HomePage");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in InitializeAsync: {ex.Message}");
                SentrySdk.CaptureException(ex);
            }
        }

        private async void OnInstagramTapped(object sender, EventArgs e)
        {
            try
            {
                Uri uri = new Uri("https://www.instagram.com/vinava_bridal/");
                await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // An unexpected error occurred. No browser may be installed on the device.
            }
        }

        private async void OnProductTapped(object sender, EventArgs e)
        {
            if (sender is StackLayout stackLayout && stackLayout.BindingContext is Product product)
            {
                int ProductId = product.Id;
                if (ProductId != null)
                {
                    await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?ProductId={ProductId}");
                    //await _viewmodel.TappedProductDetailsCommand.ExecuteAsync(product.Id);
                }
            }
        }

        private void CheckQRCode(object sender, EventArgs e)
        {
            //using (WebClient client = new WebClient())
            //{
            //    var htmlData = client.DownloadData("https://api.vietqr.io/v2/banks");
            //    var bankRawJson = Encoding.UTF8.GetString(htmlData);
            //    var listBankData = JsonConvert.DeserializeObject<Bank>(bankRawJson);
            //    var b = 1;
            //}
        }
    }
}