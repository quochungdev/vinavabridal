using DevExpress.Maui.Core.Internal;
using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class VerifyOTPPage : ContentPage
{
    public VerifyOTPPage(VerifyOTPPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void OnOTPChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        var viewModel = BindingContext as VerifyOTPPageViewModel;

        if (entry != null && viewModel != null)
        {
            var stackLayout = otpStackLayout;

            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                if (stackLayout != null)
                {
                    int currentIndex = stackLayout.Children.IndexOf(entry);

                    if (currentIndex < viewModel.Otp.Length - 1)
                    {
                        var nextEntry = stackLayout.Children[currentIndex + 1] as Entry;
                        nextEntry?.Focus();
                    }
                }
            }
            else if (string.IsNullOrEmpty(e.NewTextValue) && !string.IsNullOrEmpty(e.OldTextValue))
            {
                if (stackLayout != null)
                {
                    int currentIndex = stackLayout.Children.IndexOf(entry);

                    if (currentIndex > 0)
                    {
                        var previousEntry = stackLayout.Children[currentIndex - 1] as Entry;
                        previousEntry?.Focus();
                    }
                }
            }
        }
    }


    //private void OnOTPChanged(object sender, TextChangedEventArgs e)
    //{
    //    var viewModel = BindingContext as VerifyOTPPageViewModel;
    //    var entry = sender as Entry;
    //    if (entry != null && !string.IsNullOrEmpty(e.NewTextValue))
    //    {
    //        int currentIndex = Array.IndexOf(((sender as Entry).Parent as StackLayout).Children.ToArray(), entry);

    //        if (currentIndex < viewModel.Otp.Length - 1)
    //        {
    //            var nextEntry = FindNextEntry(currentIndex);
    //            nextEntry?.Focus();
    //        }
    //    }
    //}

    // Tìm Entry tiếp theo để chuyển focus
    private Entry FindNextEntry(int currentIndex)
    {
        if (currentIndex >= 0 && currentIndex < 5)
        {
            var stackLayout = otpStackLayout; // Thay otpStackLayout bằng x:Name của StackLayout chứa các Entry
            if (stackLayout != null)
            {
                var nextEntry = stackLayout.Children[currentIndex + 1] as Entry;
                return nextEntry;
            }
        }
        return null;
    }
    private Entry FindPreviousEntry(Entry currentEntry)
    {
        var stackLayout = otpStackLayout; 
        if (stackLayout != null)
        {
            int currentIndex = stackLayout.Children.IndexOf(currentEntry);

            if (currentIndex > 0)
            {
                var previousEntry = stackLayout.Children[currentIndex - 1] as Entry;
                return previousEntry;
            }
        }
        return null;
    }
}