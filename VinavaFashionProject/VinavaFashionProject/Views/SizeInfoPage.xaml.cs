using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class SizeInfoPage : ContentPage
{
    private SizeInfoPageViewModel _viewmodel;
    public SizeInfoPage()
    {
        InitializeComponent();
        _viewmodel = new SizeInfoPageViewModel();
        BindingContext = _viewmodel;
    }

    private void OnCupCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _viewmodel.SelectedCup = (string)radioButton.Content;
        }
    }

    private void OnCupPaddingCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && radioButton.IsChecked)
        {
            _viewmodel.SelectedPadding = (string)radioButton.Content;
        }
    }
}