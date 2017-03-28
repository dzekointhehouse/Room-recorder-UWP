using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using CodebustersAppWMU3.Models;
using CodebustersAppWMU3.Services;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CodebustersAppWMU3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SurfaceDetails : Page
    {
        private Room _currentRoom;
        private StorageFile _photo;
        public SurfaceDetails()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                _currentRoom = (Room)e.Parameter;
                // Load Surface Title
                Title.Text = _currentRoom.Surfaces[App.CurrSurface].Title;
                // Load Description
                Description.Text = _currentRoom.Surfaces[App.CurrSurface].Description;
                // Load Image to description page
                BitmapImage img = await CameraServices.ToBitmapImage(_currentRoom.Surfaces[App.CurrSurface].SurfaceImage);
                SurfaceImage.Source = img;

            }
        }

        private async void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            // Update new Values to DB and go back to previous page
            _currentRoom.Surfaces[App.CurrSurface].Title = Title.Text;
            _currentRoom.Surfaces[App.CurrSurface].Description = Description.Text;
            if (_photo != null)
            {
                _currentRoom.Surfaces[App.CurrSurface].SurfaceImage = await CameraServices.ToByteArray(_photo);
            }
        
            DatabaseRepository.UpdateSurface(_currentRoom.Surfaces[App.CurrSurface]);
            // Save then go back

            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                //By recreating the page the image can be updated immediately(if you just used
                //go back once the image would not be visible before the first swipe).
                //The double go back is to avoid creating redundant copies on the stack. 
                rootFrame.GoBack();
                rootFrame.GoBack();
                this.Frame.Navigate(typeof(CreateSurfacesPage), _currentRoom);
            }
        }

        private async void ExistingPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            StorageFile imageFile = await CameraServices.ExistingPhotosLibrary();
            BitmapImage image = await CameraServices.ToBitmapImage(imageFile);
            if (image != null)
            {
                _photo = imageFile;
                // Show the image
                SurfaceImage.Source = image;
                // Add to the current room object               
                // Update surface if you want to do without save button
               // DatabaseRepository.UpdateSurface(_currentRoom.Surfaces[App.CurrSurface]);
            }
        }
    }
}
