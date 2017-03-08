using System;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
// Required to access the core dispatcher object

// Required to access the sensor platform and the compass


namespace CodebustersAppWMU3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
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

        public MainPage()
        {
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
        }
    }
}
