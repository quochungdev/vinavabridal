using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Models;
using VinavaFashionProject.Services;
using VinavaFashionProject.Views;

namespace VinavaFashionProject.ViewModels
{
    public partial class ProductPageViewModel : ObservableObject
    {
        readonly IProductRepository _productService = new ProductService();
        [ObservableProperty]
        private string _keyword;

        [ObservableProperty]
        private int _selectedCategoryId = -1;

        [ObservableProperty]
        private string _selectedSaleDiscount;

        [ObservableProperty]
        private string _selectedOrderBy;

        [ObservableProperty]
        private int _productId;

        [ObservableProperty]
        private ObservableCollection<Product> _products;

        [ObservableProperty]
        private ObservableCollection<Category> _categories;

        [ObservableProperty]
        private ObservableCollection<string> _categoryNames;

        [ObservableProperty]
        private ObservableCollection<string> _uniqueSaleDiscounts;

        private ObservableCollection<Product> _originalProducts;

        [ObservableProperty]
        private string _selectedRadioBtn;

        [ObservableProperty]
        private bool _selected;

        private bool _isAllDataLoaded = false;
        private bool _blockLoadMore = false;

        [ObservableProperty]
        private bool _isRefreshing = false;



        [ObservableProperty]
        private bool _isBusy;
        public ProductPageViewModel()
        {

        }
        [ICommand]
        public async Task Initialize()
        {
            _keyword = null;
            _isAllDataLoaded = false;
            _products = new ObservableCollection<Product>();
            _categories = new ObservableCollection<Category>();
            _categoryNames = new ObservableCollection<string>();
            await LoadProducts();
            await LoadCategories();
            await LoadSaleDiscounts();

        }

        [ICommand]
        private async Task LoadProducts()
        {
            IsBusy = true;

            _isAllDataLoaded = false;
            SelectedCategoryId = -1;
            SelectedSaleDiscount = null;
            Keyword = null;
            _blockLoadMore = false;

            List<Product> products = await _productService.GetInitialProducts();

            if (products != null && products.Any())
            {
                Products.Clear();

                foreach (var product in products)
                {
                    await UpdateImageSource(product);
                }
                Products = new ObservableCollection<Product>(products);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Can not find Products", "Ok");
            }
            IsBusy = false;

        }

        [ICommand]
        private async Task LoadMoreProducts()

        {

            if (IsRefreshing || _isAllDataLoaded)
                return;

            IsRefreshing = true;
            await Task.Delay(1000);

            // Thực hiện tải dữ liệu trong một background thread
            await Task.Run(async () =>
            {
                var skip = Products.Count;
                var moreProducts = await _productService.GetMoreProducts(skip, 6, SelectedCategoryId, SelectedSaleDiscount, Keyword, SelectedOrderBy);
                if (moreProducts != null && moreProducts.Any())
                {
                    foreach (var product in moreProducts)
                    {
                        await UpdateImageSource(product);
                        Products.Add(product);
                    }
                }
                else
                {
                    _isAllDataLoaded = true;
                }
            });

            IsRefreshing = false;

        }
        [ICommand]
        private async Task LoadCategories()
        {
            await Task.Run(async () =>
            {
                List<Category> categories = await _productService.GetCategories();
                categories = categories.Where(c => c.IsAccessory == false).ToList();
                if (categories.Count > 0)
                {
                    Categories = new ObservableCollection<Category>(categories);
                    CategoryNames = new ObservableCollection<string>(categories.Select(c => c.Name));
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Can not find Categories", "Ok");
                }
            });

        }

        [ICommand]
        private async Task SearchProducts()
        {
            if (string.IsNullOrEmpty(Keyword))
            {
                return;
            }
            _isAllDataLoaded = false;
            _selectedCategoryId = -1;
            SelectedSaleDiscount = null;
            SelectedOrderBy = null;

            try
            {
                IsBusy = true;
                var searchedProducts = await _productService.SearchProducts(Keyword);
                if (searchedProducts != null && searchedProducts.Any())
                {
                    foreach (var product in searchedProducts)
                    {
                        await UpdateImageSource(product);
                    }

                    Products.Clear();
                    foreach (var product in searchedProducts)
                    {
                        Products.Add(product);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Result", "Không tìm thấy sản phẩm", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [ICommand]
        private async Task LoadSaleDiscounts()
        {
            await Task.Run(async () =>
            {
                List<Product> products = await _productService.GetProducts();
                if (products != null)
                {
                    var uniqueSaleDiscounts = products.Select(p => p.SaleDiscountString).Distinct().ToList();
                    if (uniqueSaleDiscounts != null && uniqueSaleDiscounts.Any())
                    {
                        List<string> formattedDiscounts = new List<string>();
                        foreach (var saleDiscount in uniqueSaleDiscounts)
                        {
                            if (decimal.TryParse(saleDiscount, out decimal discountValue))
                            {
                                string formattedDiscount = (discountValue * 100).ToString("0") + "%";
                                formattedDiscounts.Add(formattedDiscount);
                            }
                        }
                        UniqueSaleDiscounts = new ObservableCollection<string>(formattedDiscounts);

                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Can not find Products", "Ok");
                }
            });
        }

        [ICommand]
        private async Task OnSaleDiscountSelected(string selectedRadio)
        {
            //_blockLoadMore = true;

            IsBusy = true;

            SelectedSaleDiscount = selectedRadio;
            Keyword = null;
            SelectedCategoryId = -1;
            SelectedOrderBy = null;
            _isAllDataLoaded = false;

            var filteredProductsSaleDiscount = await _productService.GetProductsBySaleDiscount(selectedRadio);
            _originalProducts = new ObservableCollection<Product>(filteredProductsSaleDiscount);

            if (_originalProducts != null && _originalProducts.Any())
            {
                foreach (var product in _originalProducts)
                {
                    await UpdateImageSource(product);
                }

                Products.Clear();
                foreach (var product in _originalProducts)
                {
                    Products.Add(product);
                }
            }
            _blockLoadMore = true;
            IsBusy = false;
        }

        [ICommand]
        private async void OnAccessorySelected(string selectedRadio)
        {
            //_blockLoadMore = true;

            IsBusy = true;
            var filteredProductsAccessory = await _productService.GetProductsByAccessory(selectedRadio);
            _originalProducts = new ObservableCollection<Product>(filteredProductsAccessory);

            if (_originalProducts != null && _originalProducts.Any())
            {
                foreach (var product in _originalProducts)
                {
                    await UpdateImageSource(product);
                }

                Products.Clear();
                foreach (var product in _originalProducts)
                {
                    Products.Add(product);
                }
            }
            IsBusy = false;
            _blockLoadMore = true;

        }

        [ICommand]
        private async void OnPriceAscSelected()
        {
            //_blockLoadMore = true;

            IsBusy = true;

            SelectedCategoryId = -1;
            SelectedSaleDiscount = null;
            _isAllDataLoaded = false;
            _keyword = null;

            var filteredProductsAccessory = await _productService.GetProductsOrderedByPriceAsc();
            _originalProducts = new ObservableCollection<Product>(filteredProductsAccessory);

            if (_originalProducts != null && _originalProducts.Any())
            {
                SelectedOrderBy = "priceasc";
                foreach (var product in _originalProducts)
                {
                    await UpdateImageSource(product);
                }

                Products.Clear();
                foreach (var product in _originalProducts)
                {
                    Products.Add(product);
                }
            }
            IsBusy = false;
            _blockLoadMore = true;

        }

        [ICommand]
        private async void OnPriceDescSelected()
        {
            IsBusy = true;

            SelectedCategoryId = -1;
            SelectedSaleDiscount = null;
            _isAllDataLoaded = false;
            _keyword = null;

            var filteredProductsAccessory = await _productService.GetProductsOrderedByPriceDesc();
            _originalProducts = new ObservableCollection<Product>(filteredProductsAccessory);

            if (_originalProducts != null && _originalProducts.Any())
            {
                SelectedOrderBy = "pricedesc";
                foreach (var product in _originalProducts)
                {
                    await UpdateImageSource(product);
                }

                Products.Clear();
                foreach (var product in _originalProducts)
                {
                    Products.Add(product);
                }
            }
            IsBusy = false;
        }

        [ICommand]
        private async void OnCategorySelected(int categoryId)
        {
            //_blockLoadMore = true;
            IsBusy = true;
            SelectedCategoryId = categoryId;
            SelectedSaleDiscount = null;
            SelectedOrderBy = null;
            _isAllDataLoaded = false;
            _keyword = null;
            await Task.Run(async () =>
            {

                var productsByCategoryId = await _productService.GetProductsByCategoryId(categoryId);
                
                _originalProducts = new ObservableCollection<Product>(productsByCategoryId);

                if (_originalProducts != null && _originalProducts.Any())
                {
                    foreach (var product in _originalProducts)
                    {
                        await UpdateImageSource(product);
                    }

                    Products.Clear();
                    foreach (var product in _originalProducts)
                    {
                        Products.Add(product);
                    }
                }
            });
            IsBusy = false;
            //_blockLoadMore = true;
        }

        private async Task UpdateImageSource(Product product)
        {
            await Task.Run(async () =>
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    byte[] imageBytes = Convert.FromBase64String(product.ImageUrl);
                    ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));

                    product.ImageSourceData = imageSource;
                }
            });
        }
    }
}

