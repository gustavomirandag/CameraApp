using Plugin.Media;
using Plugin.Media.Abstractions;
using StorageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CameraApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void TakePhoto(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported || 
                !CrossMedia.Current.IsCameraAvailable)
            {
                await DisplayAlert("No Camera", "No camera detected.", "Ok");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "CameraAppAlbum",
                }
            );

            if (file == null)
                return;

            ImagePreview.Source = ImageSource.FromStream(() => file.GetStream());

            //Upload para o Azure
            BlobService blobService = new BlobService();
            var url = await blobService.UploadFileAsync("photos", Guid.NewGuid().ToString()+".jpg", file.GetStream(), "image/jpg");
            EntryUrl.Text = url;
        }

        private async void ChoosePhoto(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photo Gallery Not Found", "Photo Gallery not found", "Ok");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            ImagePreview.Source = ImageSource.FromStream(() => file.GetStream());
        }

        private async void RecordVideo(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakeVideoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                await DisplayAlert("Camera Not Detected", "No Camera Detected", "Ok");
                return;
            }

            var file = await CrossMedia.Current.TakeVideoAsync(
                new StoreVideoOptions
                {
                    SaveToAlbum = true,
                    Directory = "CameraAppAlbum",
                    Quality = VideoQuality.High,
                }
            );

            if (file == null)
                return;

            ImagePreview.Source = ImageSource.FromStream(() => file.GetStream());

            //Upload para o Azure
            BlobService blobService = new BlobService();
            var url = await blobService.UploadFileAsync("videos", Guid.NewGuid().ToString() + ".mp4", file.GetStream(), "video/mp4");
            EntryUrl.Text = url;
        }

        private async void ChooseVideo(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickVideoSupported)
            {
                await DisplayAlert("Photo Gallery not found", "Photo Gallery not found", "OK");

                return;
            }

            var file = await CrossMedia.Current.PickVideoAsync();

            if (file == null)
                return;

            ImagePreview.Source = ImageSource.FromStream(() => file.GetStream());
        }
    }
}
