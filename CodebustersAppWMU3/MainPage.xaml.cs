using System;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using CodebustersAppWMU3.Models;
using CodebustersAppWMU3.Services;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml.Navigation;

// Required to access the core dispatcher object

// Required to access the sensor platform and the compass


namespace CodebustersAppWMU3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {

            this.InitializeComponent();
            RequestBackgroundAccess();
            RegisterBackgroundTasks();
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


        private void RegisterBackgroundTasks()
        {
            bool taskRegistered = false;
            var exampleTaskName = "LocationGetter";

            // Check if the task already exists
            foreach (var test in BackgroundTaskRegistration.AllTasks)
            {
                if (test.Value.Name == exampleTaskName)
                {
                    taskRegistered = true;
                    break;
                }
            }
            // If not registered, register it
            if (!taskRegistered)
            {
                var builder = new BackgroundTaskBuilder();
                builder.Name = exampleTaskName;
                builder.TaskEntryPoint = "BackgroundTask.Class1";
                builder.SetTrigger(new TimeTrigger(15, false));

                var task = builder.Register();

                // Completion handler
                task.Completed += TaskRegistration_Completed;
            }
        }

       

        private async void RequestBackgroundAccess()
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();

            if (result == BackgroundAccessStatus.Denied)
            {
                //TODO
                ErrorMessage.DisplayErrorDialog("Cannot Sense Room");
            }
        }

        private void TaskRegistration_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
        }
    }

}


