using CommunityToolkit.Maui.Views;
using VinavaFashionProject.Models;
using VinavaFashionProject.ViewModels;

namespace VinavaFashionProject.Views;

public partial class PopupImages : Popup
{

    public PopupImages(PopupImagesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        //ProductImagess = productImages;
        //LoadImages();
    }

    //private void LoadImages()
    //{
    //    foreach (var pImage in ProductImagess)
    //    {
    //        UpdateImageSource(pImage);
    //    }

    //    // Sau khi cập nhật ImageSourceData, cập nhật BindingContext
    //    BindingContext = ProductImagess;
    //}

    //private void UpdateImageSource(ProductImage pImage)
    //{
    //    if (!string.IsNullOrEmpty(pImage.ImageUrl))
    //    {
    //        byte[] imageBytes = Convert.FromBase64String(pImage.ImageUrl);
    //        ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));

    //        pImage.ImageSourceData = imageSource;
    //    }
    //}

}