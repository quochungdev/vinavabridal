using DevExpress.Maui;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using CommunityToolkit.Maui;
using System.Collections.ObjectModel;
using VinavaFashionProject.Views;
using VinavaFashionProject.ViewModels;
using VinavaFashionProject.Models;
using Syncfusion.Maui.Core.Hosting;

namespace VinavaFashionProject
{
    public static class MauiProgram
    {
        public static ObservableCollection<User> Users { get; set; }
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()

                //// Add this section anywhere on the builder:
                //.UseSentry(options =>
                //{
                //    // The DSN is the only required setting.
                //    options.Dsn = "https://a942fe77d509cefef81fa93a707f02a4@o4506586200932352.ingest.sentry.io/4506586228719616";

                //    // Use debug mode if you want to see what the SDK is doing.
                //    // Debug messages are written to stdout with Console.Writeline,
                //    // and are viewable in your IDE's debug console or with 'adb logcat', etc.
                //    // This option is not recommended when deploying your application.
                //    options.Debug = true;

                //    // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
                //    // We recommend adjusting this value in production.
                //    options.TracesSampleRate = 1.0;

                //    // Other Sentry options can be set here.
                //})

                .UseSentry(options =>
                {
                    // The DSN is the only required setting.
                    options.Dsn = "https://a942fe77d509cefef81fa93a707f02a4@o4506586200932352.ingest.sentry.io/4506586228719616";

                    // Use debug mode if you want to see what the SDK is doing.
                    // Debug messages are written to stdout with Console.Writeline,
                    // and are viewable in your IDE's debug console or with 'adb logcat', etc.
                    // This option is not recommended when deploying your application.
                    //options.Debug = true;

                    // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
                    // We recommend adjusting this value in production.
                    options.TracesSampleRate = 1.0;

                    // Other Sentry options can be set here.
                })
                //.ConfigureEssentials(essentials =>
                //{
                //    essentials.UseVersionTracking();
                //})

                .UseMauiCommunityToolkit()

                .ConfigureSyncfusionCore()
                .UseDevExpress(useLocalization: true)
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("univia-pro-regular.ttf", "Univia-Pro");
                    fonts.AddFont("roboto-bold.ttf", "Roboto-Bold");
                    fonts.AddFont("roboto-regular.ttf", "Roboto");
                });

            //builder.Services.AddMicrosoftIdentityWebApi(Configuration, configSectionName: "Authentication:AzureAd", jwtBearerScheme: "AzureAd")
            //               .EnableTokenAcquisitionToCallDownstreamApi()
            //               .AddInMemoryTokenCaches();

            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomePageViewModels>();

            builder.Services.AddSingleton<SizeInfoPage>();
            builder.Services.AddSingleton<SizeInfoPageViewModel>();

            builder.Services.AddSingleton<CollectionPage>();

            builder.Services.AddTransient<MemberPage>();
            builder.Services.AddTransient<MemberPageViewModels>();

            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<RegisterPageViewModel>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginPageViewModel>();

            builder.Services.AddSingleton<AccountInfoPage>();
            builder.Services.AddSingleton<AccountInfoPageViewModel>();

            builder.Services.AddSingleton<ForgotPasswordPage>();
            builder.Services.AddSingleton<ForgotPasswordViewModel>();

            builder.Services.AddSingleton<VerifyOTPPage>();
            builder.Services.AddSingleton<VerifyOTPPageViewModel>();

            builder.Services.AddSingleton<ResetPasswordPage>();

            builder.Services.AddSingleton<ContactPage>();


            builder.Services.AddSingleton<ProductPage>();
            builder.Services.AddSingleton<ProductPageViewModel>();

            builder.Services.AddSingleton<ProductDetailPage>();
            builder.Services.AddSingleton<ProductDetailPageViewModel>();

            builder.Services.AddSingleton<PopupImages>();
            builder.Services.AddSingleton<PopupImagesViewModel>();

            builder.Services.AddSingleton<PopupSizeImage>();

            builder.Services.AddTransient<CartPage>();
            builder.Services.AddTransient<CartPageViewModel>();

            builder.Services.AddTransient<PaymentPage>();
            builder.Services.AddTransient<PaymentPageViewModel>();

            builder.Services.AddTransient<OrderPage>();
            builder.Services.AddTransient<OrderPageViewModel>();

            builder.Services.AddTransient<OrderDetailPage>();
            builder.Services.AddTransient<OrderDetailPageViewModel>();

            builder.Services.AddTransient<AddAddressPage>();
            builder.Services.AddTransient<AddressPageViewModel>();

            builder.Services.AddTransient<PaymentSuccessPage>();
            builder.Services.AddTransient<PaymentPayPalSuccessPage>();
            builder.Services.AddTransient<PaymentSuccessPageViewModel>();

            builder.Services.AddTransient<PopupPayPalInfoViewModel>();
            builder.Services.AddTransient<PopupPayPalInfo>();

            builder.Services.AddTransient<FavouriteProductPage>();
            builder.Services.AddTransient<FavouriteProductPageViewModel>();

            return builder.Build();
        }
    }
}
