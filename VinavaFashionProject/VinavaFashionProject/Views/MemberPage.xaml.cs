using CommunityToolkit.Mvvm.Input;
using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class MemberPage : ContentPage
{
    private MemberPageViewModels _viewModel;

    public MemberPage(MemberPageViewModels vm)
    {
        InitializeComponent();
        _viewModel = vm;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (App.user != null)
        {
            if (_viewModel == null)
            {
                _viewModel = new MemberPageViewModels();
                BindingContext = _viewModel;
            }
            _viewModel.FullName = App.user.FullName;
            _viewModel.Email = App.user.Email;
        }
    }

}