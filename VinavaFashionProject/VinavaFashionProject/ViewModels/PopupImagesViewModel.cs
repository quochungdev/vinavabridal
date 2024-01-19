using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinavaFashionProject.Models;

namespace VinavaFashionProject.ViewModels
{
    public partial class PopupImagesViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<ProductImage> _productImages;

        public PopupImagesViewModel(List<ProductImage> productImages)
        {
            ProductImages = productImages;
            LoadImages();
        }
        private void LoadImages()
        {
            foreach (var pImage in ProductImages)
            {
                UpdateImageSource(pImage);
            }
        }
        private void UpdateImageSource(ProductImage pImage)
        {
            if (!string.IsNullOrEmpty(pImage.ImageUrl))
            {
                byte[] imageBytes = Convert.FromBase64String(pImage.ImageUrl);
                ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));

                pImage.ImageSourceData = imageSource;
            }
        }
    }
}
