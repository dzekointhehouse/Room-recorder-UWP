using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using CodebustersAppWMU3.Models;
using CodebustersAppWMU3.Services;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CodebustersAppWMU3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateSurfacesPage : Page
    {

        private Room _currentRoom;
        private CameraServices _photoService;
        public CreateSurfacesPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            SwipeConfiguration();
            SwipeBlock.Text = SurfaceOptions.SurfaceSide(App.CurrSurface);
            Application.Current.Suspending += new SuspendingEventHandler(App_Suspending);
            Application.Current.Resuming += new EventHandler<Object>(App_Resuming);

        }

        private void App_Resuming(object sender, object e)
        {
            ApplicationDataContainer appSettings = ApplicationData.Current.LocalSettings;

            string title = (string)appSettings.Values["CurrentPage"];
            _currentRoom = DatabaseRepository.GetRoom(title);
           // ErrorMessage.DisplayErrorDialog("Resuming" + title);
        }

        private void App_Suspending(object sender, SuspendingEventArgs e)
        {
            ApplicationDataContainer Appsettings = ApplicationData.Current.LocalSettings;
            Appsettings.Values["CurrentPage"] = _currentRoom.Title;
            Appsettings.Values["TimeStamp"] = DateTime.Now.Hour ;
            ErrorMessage.DisplayErrorDialog("Suspending" + _currentRoom.Title);
        }
        protected override async void OnNavigatedTo(NavigationEventArgs eventArgs)
        {
            if (eventArgs.Parameter != null)
            {
                _photoService = new CameraServices();
                _currentRoom = (Room) eventArgs.Parameter;
                TitleBlock.Text = _currentRoom.Title;

                var b = _currentRoom.Surfaces[App.CurrSurface].SurfaceImage;
                var img = await CameraServices.ToBitmapImage(b);

                SurfaceImage.Source = img;
            }
        }
   
        private async void CameraButton_Click(object sender, RoutedEventArgs e)
        {
            // Get camera and return a picture or null
            StorageFile photo = await _photoService.TakePicture();

            if (photo != null)
            {
                // Convert display and save
                SurfaceImage.Source = await CameraServices.ToBitmapImage(photo);
                _photoService.SavePhoto(photo, _currentRoom, App.CurrSurface);

            }
        }

        /* 
         * Beautiful swiping functionality developed by Elvir and Nick of course,
         * It takes into consideration that a sweep wont be perfectly horizontal or
         * vertical. Oh, and yes it is a four way swipe.
         */
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
                    if (App.CurrSurface != 0)
                    {
                        App.CurrSurface--;
                    }
                    else
                    {
                        App.CurrSurface = 3;
                    }
                }
                else if (x1 < x2 && horizontal > vertical)
                {
                    if (App.CurrSurface < 3)
                    {
                        App.CurrSurface++;
                    }
                    else
                    {
                        App.CurrSurface = 0;
                    }
                }
                else if (y1 > y2 && horizontal < vertical)
                {
                    App.CurrSurface = 4;

                }
                else if (y1 < y2 && horizontal < vertical)
                {
                    App.CurrSurface = 5;
                }
                // Surface name
                SwipeBlock.Text = SurfaceOptions.SurfaceSide(App.CurrSurface);
                // Update the current room
                _currentRoom = DatabaseRepository.GetRoom(_currentRoom.Title);
                // Converting from bytearray to BitmapImage and showing it.
                var img = await CameraServices.ToBitmapImage(_currentRoom.Surfaces[App.CurrSurface].SurfaceImage);
                SurfaceImage.Source = img;
            };
        }

        private void SurfaceDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SurfaceDetails), _currentRoom);
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
