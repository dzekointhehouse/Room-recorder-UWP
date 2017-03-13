using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CodebustersAppWMU3.Models;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CodebustersAppWMU3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateSurfacesPage : Page
    {
        private int _currSurface;
        private Room _currentRoom;
        private enum Days { Left = 0, Front, Right, Back};
        public CreateSurfacesPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            SwipeConfiguration();
            SwipeBlock.Text = SurfaceOptions.SurfaceSide(_currSurface);
        }

        private async void GetCamera()
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size();

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo == null)
            {
                // User cancelled photo capture
                return;
            }

            StorageFolder destinationFolder =
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("RoomDocumentation",
                    CreationCollisionOption.OpenIfExists);

            // Approriately saving the picture file with the room title and current room SurfaceSide
            // for identification later.

            var surfaceFileName = _currentRoom.Title + SurfaceOptions.SurfaceSide(_currSurface);

            await photo.CopyAsync(
                destinationFolder,
                surfaceFileName,
                NameCollisionOption.ReplaceExisting);
            await photo.DeleteAsync();




        }


        protected override void OnNavigatedTo(NavigationEventArgs eventArgs)
        {


            if (eventArgs.Parameter != null)
            {
                _currentRoom = (Room) eventArgs.Parameter;
                TitleBlock.Text = _currentRoom.Title;
            }


        }

        private async void CheckIfPictureExist()
        {
            var surfaceFileName = _currentRoom.Title + SurfaceOptions.SurfaceSide(_currSurface);
            
            StorageFolder destinationFolder =
                await ApplicationData.Current.LocalFolder.CreateFolderAsync("RoomDocumentation",
                    CreationCollisionOption.OpenIfExists);
            try
            {
                IRandomAccessStream stream =
                    await destinationFolder.GetFileAsync(surfaceFileName).GetResults().OpenAsync(FileAccessMode.Read);
            

            if (stream.CanRead)
            {
 
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();

                    SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Premultiplied);

                    SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
                    await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

                    SurfaceImage.Source = bitmapSource;
   
            }
            }
            catch
            {

            
            BitmapImage bitmapImage = new BitmapImage(new Uri(this.BaseUri, "/Assets/Square150x150Logo.scale-200.png"));

                SurfaceImage.Source = bitmapImage;
            }

        }
        private void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            GetCamera();
        }

        private void SwipeConfiguration()
        {

            int x1 = 0, x2 = 0, y1 = 0, y2 = 0, horizontal, vertical;


            ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            ManipulationStarted += (s, e) =>
            {
                x1 = (int)e.Position.X;
                y1 = (int)e.Position.Y;
            };
            ManipulationCompleted += (s, e) =>
            {
                x2 = (int)e.Position.X;
                y2 = (int)e.Position.Y;

                horizontal = Math.Abs(x2 - x1);
                vertical = Math.Abs(y2 - y1);


                if (x1 > x2 && horizontal > vertical)
                {
                    if (_currSurface != 0)
                    {
                        _currSurface--;
                    }
                    else
                    {
                        _currSurface = 3;
                    }
                }
                else if (x1 < x2 && horizontal > vertical)
                {
                    if (_currSurface < 3)
                    {
                        _currSurface++;
                    }
                    else
                    {
                        _currSurface = 0;
                    }
                }
                else if (y1 > y2 && horizontal < vertical)
                {
                    _currSurface = 4;

                }
                else if (y1 < y2 && horizontal < vertical)
                {
                    _currSurface = 5;
                }

                SwipeBlock.Text = SurfaceOptions.SurfaceSide(_currSurface);
                CheckIfPictureExist();
            };
        }
    }
}
