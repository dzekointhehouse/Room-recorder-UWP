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
        private CameraServices _photoService;
        private enum Sides { Left = 0, Front, Right, Back};
        public CreateSurfacesPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            SwipeConfiguration();
            SwipeBlock.Text = SurfaceOptions.SurfaceSide(_currSurface);
        }


        protected override void OnNavigatedTo(NavigationEventArgs eventArgs)
        {


            if (eventArgs.Parameter != null)
            {
                _photoService = new CameraServices();
                _currentRoom = (Room) eventArgs.Parameter;
                TitleBlock.Text = _currentRoom.Title;
            }


        }

   
        private async void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            StorageFile photo = await _photoService.GetCamera();

            if (photo != null)
            {
                // Assign photo to currentRoom -> current surface -> room photo
                _currentRoom.Surfaces[_currSurface].SurfaceImage = photo;
            }
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
            ManipulationCompleted += async (s, e) =>
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
                BitmapImage img = await _photoService.CheckIfPictureExist(_currentRoom.Title, _currSurface, this.BaseUri);
                SurfaceImage.Source = img;

            };
        }

        private void SurfaceDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SurfaceDetails), _currentRoom);
        }
    }
}
