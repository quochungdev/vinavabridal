using VinavaFashionProject.Models;
using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject
{
    public partial class App : Application
    {
        public static User user;
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("");
        }
    }
}