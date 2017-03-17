using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace CodebustersAppWMU3.Models
{
    class CameraServices
    {
        private StorageFolder _destinationFolder;

        public CameraServices()
        {
            InitiateFileFolder();
        }

        private async void InitiateFileFolder()
        {
            _destinationFolder =
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("RoomDocumentation",
                    CreationCollisionOption.OpenIfExists);
        }

        public async Task<StorageFile> GetCamera()
        {
            CameraCaptureUI captureUi = new CameraCaptureUI();
            captureUi.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUi.PhotoSettings.CroppedSizeInPixels = new Size();

            StorageFile photo = await captureUi.CaptureFileAsync(CameraCaptureUIMode.Photo);

            return photo;
        }

        public async void SavePhoto(StorageFile photo, string currentRoom, int currentSurface)
        {
            var surfaceFileName = currentRoom + SurfaceOptions.SurfaceSide(currentSurface);
            await photo.CopyAsync(
                _destinationFolder,
                surfaceFileName,
                NameCollisionOption.ReplaceExisting);
            await photo.DeleteAsync();
        }

        public async Task<BitmapImage> CheckIfPictureExist(string roomTitle, int surface, Uri baseUri)
        {
            var surfaceFileName = roomTitle + SurfaceOptions.SurfaceSide(surface);
            BitmapImage img;

            StorageFolder destinationFolder =
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("RoomDocumentation",
                    CreationCollisionOption.OpenIfExists);
            try
            {

                BitmapImage bitmapImage = new BitmapImage();
                StorageFile file = await destinationFolder.GetFileAsync(surfaceFileName);
                var image = await Windows.Storage.FileIO.ReadBufferAsync(file);
                Uri uri = new Uri(file.Path);
                img = new BitmapImage(new Uri(file.Path));
                return img;

            }
            catch
            {

                // Creates an bitmapimage for the page using this class. It needs the baseuri from the page and asset image.
                // This is returned if no image is found.
                img = new BitmapImage(new Uri(baseUri, "/Assets/Square150x150Logo.scale-200.png"));
                return img;

            }
        }
    }
}
