using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace VinavaFashionProject.ViewModels
{
    public partial class SizeInfoPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _name = App.user.Username;

        [ObservableProperty]
        private string _email = App.user.Email;

        [ObservableProperty]
        private string _bust;

        [ObservableProperty]
        private string _waist;

        [ObservableProperty]
        private string _hips;

        [ObservableProperty]
        private string _cupHorizontal;

        [ObservableProperty]
        private string _cupVertical;

        [ObservableProperty]
        private string _height;

        [ObservableProperty]
        private string _legHeight;

        [ObservableProperty]
        private string _skirtLenght;

        [ObservableProperty]
        private string _heelsHeight;

        [ObservableProperty]
        private string _selectedCup = "";

        [ObservableProperty]
        private string _selectedPadding = "";

        [ICommand]
        private async Task SendEmail()
        {
            try
            {
                if (!string.IsNullOrEmpty(Name) &&
            !string.IsNullOrEmpty(Email) &&
            !string.IsNullOrEmpty(Bust) &&
            !string.IsNullOrEmpty(Waist) &&
            !string.IsNullOrEmpty(Hips) &&
            !string.IsNullOrEmpty(CupHorizontal) &&
            !string.IsNullOrEmpty(CupVertical) &&
            !string.IsNullOrEmpty(Height) &&
            !string.IsNullOrEmpty(LegHeight) &&
            !string.IsNullOrEmpty(SkirtLenght) &&
            !string.IsNullOrEmpty(HeelsHeight) &&
            !string.IsNullOrEmpty(SelectedCup) &&
            !string.IsNullOrEmpty(SelectedPadding))
                {
                    string userEmail = Email;

                    string myEmail = "vinavabridalapp@gmail.com";
                    string passwordApp = "lsxugluhljrogpzn";

                    MailMessage mail = new MailMessage();
                    SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress(Email);
                    mail.To.Add("vinavabridalapp@gmail.com");
                    mail.Subject = "Customer Size Information";

                    string htmlBody = $@"
            <html>
            <body>
                <h1>Size information of customer whose Username: {Name}</h1>
                <p>Name: {Name}</p>
                <p>Email: {Email}</p>
                <p>Bust: {Bust}</p>
                <p>Waist: {Waist}</p>
                <p>Hips: {Hips}</p>
                <p>Cup Horizontal: {CupHorizontal}</p>
                <p>Cup Vertical: {CupVertical}</p>
                <p>Height: {Height}</p>
                <p>Leg Height: {LegHeight}</p>
                <p>Skirt Length: {SkirtLenght}</p>
                <p>Heels Height: {HeelsHeight}</p>
                <p>Cup : {SelectedCup}</p>
                <p>Cup Padding: {SelectedPadding}</p>
            </body>
            </html>
        ";
                    mail.IsBodyHtml = true;
                    mail.Body = htmlBody;

                    smtpServer.Port = 587;
                    smtpServer.EnableSsl = true; // Kết nối bảo mật SSL/TLS
                    smtpServer.Credentials = new NetworkCredential(myEmail, passwordApp);
                    smtpServer.Send(mail);

                    // Thông báo gửi email thành công
                    await Shell.Current.DisplayAlert("Success", "Email has been sent successfully!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Please fill in all required information!", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // Xử lý ngoại lệ khi gửi email thất bại
                await Shell.Current.DisplayAlert("Error", "Failed to send email!", "OK");
            }
        }
    }
}
