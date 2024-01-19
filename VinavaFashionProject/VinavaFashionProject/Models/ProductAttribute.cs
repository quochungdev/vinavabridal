using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.CodeAnalysis;
using System.ComponentModel;

namespace VinavaFashionProject.Models
{
    public class ProductAttribute : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int AttributeId { get; set; }

        public virtual Attribute Attribute { get; set; }

        private bool isVisibleBorder;
        public bool IsVisibleBorder
        {
            get => isVisibleBorder;
            set
            {
                if (isVisibleBorder != value)
                {
                    isVisibleBorder = value;
                    OnPropertyChanged(nameof(IsVisibleBorder)); 
                    OnPropertyChanged(nameof(BorderThickness)); 
                }
            }
        }
        public int BorderThickness => IsVisibleBorder ? 2 : 0;

        private int _borderWidth;
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                if (_borderWidth != value)
                {
                    _borderWidth = value;
                    OnPropertyChanged(nameof(BorderWidth));
                }
            }
        }


        // Sự kiện PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
