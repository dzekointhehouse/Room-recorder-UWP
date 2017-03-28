using System;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using CodebustersAppWMU3.Models;
using CodebustersAppWMU3.Services;
using Windows.ApplicationModel.Background;

// Required to access the core dispatcher object

// Required to access the sensor platform and the compass


namespace CodebustersAppWMU3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //SystemCondition userCondition = new SystemCondition(SystemConditionType.UserPresent);
        //TimeTrigger quarterTimer = new TimeTrigger(15, false);
        //BackgroundTaskRegistration task = new BackgroundTaskRegistration("something", "Background Coordinate", quarterTimer, userCondition);// varför funk inte?


        //private Compass _compass; // Our app' s compass object

        //// This event handler writes the current compass reading to
        //// the textblocks on the app' s main page.

        //private async void ReadingChanged(object sender, CompassReadingChangedEventArgs e)
        //{
        //    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    {
        //        CompassReading reading = e.Reading;
        //        txtMagnetic.Text = String.Format("{0,5:0.00}", reading.HeadingMagneticNorth);
        //        if (reading.HeadingTrueNorth.HasValue)
        //            txtNorth.Text = String.Format("{0,5:0.00}", reading.HeadingTrueNorth);
        //        else
        //            txtNorth.Text = "No reading.";
        //    });
        //}
        bool taskRegistered = false;
        public MainPage()
        {
            var builder = new BackgroundTaskBuilder();
            builder.Name = "LocationGetter";
            builder.TaskEntryPoint = "GeoLocation.Class1";
            builder.SetTrigger(new TimeTrigger(15,false));
            //  builder.AddCondition(new SystemCondition(SystemConditionType.UserPresent));
            BackgroundTaskRegistration task = builder.Register();
            foreach (var test in BackgroundTaskRegistration.AllTasks)
            {
                if (test.Value.Name == builder.Name)
                {
                    taskRegistered = true;
                    break;
                }
            }
            this.InitializeComponent();        

            //_compass = Compass.GetDefault(); // Get the default compass object

            //// Assign an event handler for the compass reading-changed event
            //if (_compass != null)
            //{
            //    // Establish the report interval for all scenarios
            //    uint minReportInterval = _compass.MinimumReportInterval;
            //    uint reportInterval = minReportInterval > 16 ? minReportInterval : 16;
            //    _compass.ReportInterval = reportInterval;
            //    _compass.ReadingChanged += new TypedEventHandler<Compass, CompassReadingChangedEventArgs>(ReadingChanged);
            //}            
            // GetCoordinate();  
        }
        public async void GetCoordinate()
        {
            var locator = new Geolocator();
            locator.DesiredAccuracyInMeters = 1;
            var position = await locator.GetGeopositionAsync();
            var myposition = position.Coordinate.Point;
            var dummy = position.Coordinate.Point;
        }

        private void NewRoom_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateRoomPage));
        }

        private void ExistingRoom_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var name = RoomNameBox.Text;

            Room room = DatabaseRepository.GetRoom(name);

            if (room != null)
            {
                Frame.Navigate(typeof(CreateSurfacesPage), room);
            }
            else
            {
                ErrorMessage.DisplayErrorDialog("Room not found.");
            }
        }
    }
    }


