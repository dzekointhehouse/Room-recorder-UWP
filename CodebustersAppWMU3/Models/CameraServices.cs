using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using CodebustersAppWMU3.Services;

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

        public async void SavePhoto(StorageFile photo, string roomTitle, int currentSurface, Surface surface)
        {
            //var surfaceFileName = currentRoom + SurfaceOptions.SurfaceSide(currentSurface);
            //await photo.CopyAsync(
            //    _destinationFolder,
            //    surfaceFileName,
            //    NameCollisionOption.ReplaceExisting);
            //await photo.DeleteAsync();
          Byte[] bytesImg = await AsByteArray(photo);
            DatabaseRepository.SaveRoomSurface(roomTitle, currentSurface, surface);
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

        public static async Task<byte[]> AsByteArray(StorageFile file)
        {
            IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read);
            var reader = new Windows.Storage.Streams.DataReader(fileStream.GetInputStreamAt(0));
            await reader.LoadAsync((uint)fileStream.Size);

            byte[] pixels = new byte[fileStream.Size];

            reader.ReadBytes(pixels);

            return pixels;
        }

        public static async Task<BitmapImage> ToBitmapImage(byte[] byteArray, Uri baseUri)
        {
            if(byteArray != null) { 
            using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                // Writes the image byte array in an InMemoryRandomAccessStream
                // that is needed to set the source of BitmapImage.
                using (DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(byteArray);
                    await writer.StoreAsync();
                }

                var image = new BitmapImage();
                await image.SetSourceAsync(ms);

                return image;
            }
            }
            // Creates an bitmapimage for the page using this class. It needs the baseuri from the page and asset image.
            // This is returned if no image is found.
            BitmapImage img = new BitmapImage(new Uri(baseUri, "/Assets/Square150x150Logo.scale-200.png"));
            return img;
        }

        public static async Task<BitmapImage> AsBitmapImage(StorageFile file)
        {
            var stream = await file.OpenAsync(FileAccessMode.Read);
            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(stream);
            return bitmapImage;
        }
  

    }
}
