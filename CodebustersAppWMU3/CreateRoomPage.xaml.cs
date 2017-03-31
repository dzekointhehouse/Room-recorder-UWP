using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CodebustersAppWMU3.Models;
using CodebustersAppWMU3.Services;
using Microsoft.EntityFrameworkCore;


namespace CodebustersAppWMU3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateRoomPage : Page
    {
        public CreateRoomPage()
        {
            this.InitializeComponent();
            GetRoomLocation();

        }

        /* 
         * Gets the users location when creating the room, the method has switch-cases depending on the
         * access status. if we are allowed to use the position we create an instance of geolocator and locate
         * the "rooms" position. Otherwise we use a default location value {0,0}
         */
        public async void GetRoomLocation()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();

            try
            {

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 50 };

                        // Subscribe to the PositionChanged event to get location updates.
                        geolocator.PositionChanged += OnPositionChanged;
                        var position = await geolocator.GetGeopositionAsync();
                        var myposition = position.Coordinate.Point;

                        LatiValue.Text = myposition.Position.Latitude.ToString();
                        LongtValue.Text = myposition.Position.Longitude.ToString();
                        break;

                    case GeolocationAccessStatus.Denied:
                        ErrorMessage.DisplayErrorDialog("Location access denied");
                        LatiValue.Text = "0.0";
                        LongtValue.Text = "0.0";
                        break;

                    case GeolocationAccessStatus.Unspecified:
                        ErrorMessage.DisplayErrorDialog("Some kind of error occured, please try again!");
                        LatiValue.Text = "0.0";
                        LongtValue.Text = "0.0";
                        break;
                }
            }
            catch (Exception)
            {
                LatiValue.Text = "0.0";
                LongtValue.Text = "0.0";
            }

        }

        /* 
         * Keeps track of the current phone position,
         * updates when the position is changed.
         */
        private async void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var myposition = args.Position.Coordinate.Point;

                LatiValue.Text = myposition.Position.Latitude.ToString();
                LongtValue.Text = myposition.Position.Longitude.ToString();
            });

        }

        /*
         * Here is the interesting part for creating new rooms. We start of by checking if we 
         * have gotten our location values, If we have then we continue on to check the input
         * values and try to create the room. We get an status message back from the database
         * if successful or else not (null).
         */
        private void CreateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get position values.
                double lat = double.Parse(LatiValue.Text);
                double longt = double.Parse(LongtValue.Text);
            }
            catch
            {
                // Do nothing if incorrect position values!
                return;
            }
            // Check if title already exists!
            if (!IsTitleAllowed(Title.Text))
            {
                ErrorMessage.DisplayErrorDialog("Please, check yo title again!");
                return;
            }
            // If is empty
            if (Description.Text == "")
            {
                ErrorMessage.DisplayErrorDialog("Please, check yo description again!");
                return;
            }
            // Creates the room if it doesn't exist
            var room = DatabaseRepository.CreateRoom(Title.Text, Description.Text,
                0.0, double.Parse(LatiValue.Text), double.Parse(LongtValue.Text));
            // Navigate to next page if successful
            if (room != null)
            {
                Frame.Navigate(typeof(CreateSurfacesPage), room);
            }
            else
            {
                ErrorMessage.DisplayErrorDialog("Error, room probably already exists!");
            }

        }

        /* 
         * Used to make sure the text parameter matches the
         * regex expression
         */
        private static bool IsTitleAllowed(string text)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9]{1,}$"); //letters, whitespace and more than 0 chars
            return regex.IsMatch(text);
        }
    }
}
