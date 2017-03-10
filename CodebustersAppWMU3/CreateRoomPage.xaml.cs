using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CodebustersAppWMU3.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CodebustersAppWMU3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateRoomPage : Page
    {
        private Geolocator locator = new Geolocator { ReportInterval = 2000 };
        public CreateRoomPage()
        {
            this.InitializeComponent();
            GetCoordinate();

        }

        public async void GetCoordinate()
        {
            

            locator.PositionChanged += OnPositionChanged;
            locator.DesiredAccuracyInMeters = 1;
            var position = await locator.GetGeopositionAsync();
            var myposition = position.Coordinate.Point;

            LatiValue.Text = myposition.Position.Latitude.ToString();
            LongtValue.Text = myposition.Position.Longitude.ToString();
        }

        private async void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            var position = await locator.GetGeopositionAsync();
            var myposition = position.Coordinate.Point;

           //LatiValue.Text = myposition.Position.Latitude.ToString();
           //LongtValue.Text = myposition.Position.Longitude.ToString();
        }

        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            double lat = double.Parse(LatiValue.Text);
            double longt = double.Parse(LongtValue.Text);

            var newRoom = new Room()
            {
                Title = Title.Text,
                Description = Description.Text,
                Longt = double.Parse(LongtValue.Text),
                Lat = double.Parse(LatiValue.Text)
            };

            Frame.Navigate(typeof(CreateSurfacesPage), newRoom);
        }
    }
}
