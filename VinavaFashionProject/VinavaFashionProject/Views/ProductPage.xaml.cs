using DevExpress.Maui.Controls;
using DevExpress.Maui.Core;
using DevExpress.Maui.Editors;
using Microsoft.Maui.Controls;
using Sentry;
using System.Globalization;
using VinavaFashionProject.Models;
using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ProductPage : ContentPage
    {
        private ProductPageViewModel _viewmodel;
        private bool _isProcessing = false;
        public ProductPage()
        {
            InitializeComponent();
            _viewmodel = new ProductPageViewModel();
            BindingContext = _viewmodel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await _viewmodel.InitializeCommand.ExecuteAsync(null);
            SentrySdk.CaptureMessage("Wrong ProductPage");
        }


        private void OnClickedRadioButton(object sender, EventArgs e)
        {
            if (_isProcessing)
            {
                return;
            }

            RadioButton rdb = sender as RadioButton;
            if (rdb.IsChecked)
            {
                _isProcessing = true;
                string content = rdb?.Content as string;
                if (decimal.TryParse(content.Replace("%", ""), out decimal discountValue))
                {
                    decimal formattedValue = discountValue / 100;
                    string formattedString = formattedValue.ToString("0.00");
                    _viewmodel.OnSaleDiscountSelectedCommand.Execute(formattedString);
                }
                _isProcessing = false;
            }
        }
        private void OnAccessoryRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (_isProcessing)
            {
                return;
            }

            //RadioButton rdb = sender as RadioButton;
            if (sender is RadioButton rdb && rdb.IsChecked)
            {
                _isProcessing = true;
                _viewmodel.OnAccessorySelectedCommand.Execute(rdb.Content);
                _isProcessing = false;
            }
        }
        private void OnPriceAscRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (_isProcessing)
            {
                return;
            }

            //RadioButton rdb = sender as RadioButton;
            if (sender is RadioButton rdb && rdb.IsChecked)
            {
                _isProcessing = true;
                _viewmodel.OnPriceAscSelectedCommand.Execute(null);
                _isProcessing = false;
            }

        }
        private void OnPriceDescRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (_isProcessing)
            {
                return;
            }

            //RadioButton rdb = sender as RadioButton;
            if (sender is RadioButton rdb && rdb.IsChecked)
            {
                _isProcessing = true;
                _viewmodel.OnPriceDescSelectedCommand.Execute(null);
                _isProcessing = false;
            }

        }
        private void OnCategoryRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.IsChecked)
            {
                if (radioButton.BindingContext is Category category)
                {
                    int categoryId = category.Id;
                    _viewmodel.OnCategorySelectedCommand.Execute(categoryId);
                }
            }
        }

        private void Reset_Clicked(object sender, EventArgs e)
        {
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

        private void OnFilterChipGroupTap(object sender, ChipEventArgs e)
        {
            filterTabView.SelectedItemIndex = filterChipGroup.Chips.IndexOf(e.Chip);
            UpdateBottmSheetState(filterTabView.SelectedItemIndex);
        }

        private void OnCloseBottomSheetClicked(object sender, EventArgs e)
        {
            filterBottomSheet.State = BottomSheetState.Hidden;
        }

        private void FilterTabHeaderTapped(object sender, ItemHeaderTappedEventArgs e)
        {
            UpdateBottmSheetState(e.Index);
        }
        void UpdateBottmSheetState(int selectedIndex)
        {
            if (selectedIndex == 4)
                filterBottomSheet.State = BottomSheetState.FullExpanded;
            else
                filterBottomSheet.State = BottomSheetState.HalfExpanded;
        }
    }

    public class IsFilterEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FilterValueInfo filterInfo && filterInfo != null)
            {
                return filterInfo.Count == 0;
            }
            else if (value is int selectedFilterItems)
            {
                return selectedFilterItems == 0;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

