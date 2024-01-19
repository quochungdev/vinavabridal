using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinavaFashionProject.Models
{
    public partial class OrderDetail : ObservableObject
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity)); // Notify property changed
                }
            }
        }

        public decimal Price { get; set; }

        public decimal Total { get; set; }
        public Product Product { get; set; }

        public List<ProductAttribute> ColorAttributes
        {
            get
            {
                return Product?.ProductAttributes?.Where(attr => attr.Attribute.AttributeName == "color").ToList();
            }
            set { }
        }

        public List<ProductAttribute> SizeAttributes
        {
            get
            {
                return Product?.ProductAttributes?.Where(attr => attr.Attribute.AttributeName == "size").ToList();
            }
        }

        private ProductAttribute _selectedColor;
        public ProductAttribute SelectedColor
        {
            get => _selectedColor;
            set
            {
                SetProperty(ref _selectedColor, value);
            }
        }

        private ProductAttribute _selectedSize;
        public ProductAttribute SelectedSize
        {
            get => _selectedSize;
            set
            {
                SetProperty(ref _selectedSize, value);
            }
        }
    }
}
