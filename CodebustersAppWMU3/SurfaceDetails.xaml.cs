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
                Title.Text = _currentRoom.Surfaces[App._currSurface].Title;
                // Load Description
                Description.Text = _currentRoom.Surfaces[App._currSurface].Description;
                // Load Image to description page
                BitmapImage img = await CameraServices.ToBitmapImage(_currentRoom.Surfaces[App._currSurface].SurfaceImage);
                SurfaceImage.Source = img;

            }
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            // Update new Values to DB and go back to previous page
            _currentRoom.Surfaces[App._currSurface].Title = Title.Text;
            _currentRoom.Surfaces[App._currSurface].Description = Description.Text;
            DatabaseRepository.UpdateSurface(_currentRoom.Surfaces[App._currSurface]);
            // Save then go back
            this.Frame.Navigate(typeof(CreateSurfacesPage), _currentRoom);
        }
    }
}
