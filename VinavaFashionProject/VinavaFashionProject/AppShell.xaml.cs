using VinavaFashionProject.Views;

namespace VinavaFashionProject
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
            Routing.RegisterRoute(nameof(VerifyOTPPage), typeof(VerifyOTPPage));
            Routing.RegisterRoute(nameof(ResetPasswordPage), typeof(ResetPasswordPage));
            Routing.RegisterRoute(nameof(SizeInfoPage), typeof(SizeInfoPage));

            Routing.RegisterRoute(nameof(MemberPage), typeof(MemberPage));
            Routing.RegisterRoute(nameof(AccountInfoPage), typeof(AccountInfoPage));

            Routing.RegisterRoute(nameof(ProductPage), typeof(ProductPage));
            Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));

            Routing.RegisterRoute(nameof(OrderPage), typeof(OrderPage));
            Routing.RegisterRoute(nameof(OrderDetailPage), typeof(OrderDetailPage));

            Routing.RegisterRoute(nameof(CollectionPage), typeof(CollectionPage));

            Routing.RegisterRoute(nameof(PopupImages), typeof(PopupImages));
            Routing.RegisterRoute(nameof(PopupPayPalInfo), typeof(PopupPayPalInfo));

            Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
            Routing.RegisterRoute(nameof(FavouriteProductPage), typeof(FavouriteProductPage));

            Routing.RegisterRoute(nameof(AddAddressPage), typeof(AddAddressPage));

            Routing.RegisterRoute(nameof(PaymentPage), typeof(PaymentPage));
            Routing.RegisterRoute(nameof(PaymentSuccessPage), typeof(PaymentSuccessPage));
            Routing.RegisterRoute(nameof(PaymentPayPalSuccessPage), typeof(PaymentPayPalSuccessPage));

        }
    }
}
