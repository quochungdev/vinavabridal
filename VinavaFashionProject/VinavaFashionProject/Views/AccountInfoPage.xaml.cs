using VinavaFashionProject.Models;
using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;
//[QueryProperty(nameof(MemberPage), "Settings")]
public partial class AccountInfoPage : ContentPage
{
    MemberPageViewModels memberPageVM;
    public MemberPageViewModels MemberPageVM
    {
        get => this.memberPageVM;
        set => this.memberPageVM = value;
    }
    public AccountInfoPage(AccountInfoPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

}