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
            // Access and register background tasks.
            RequestBackgroundAccess();
            RegisterBackgroundTasks();
        }

        private void NewRoom_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateRoomPage));
        }

        /* 
         * Navigates to createsurfaces page if an existing room is found, otherwise it shows
         * and error prompt.
         */
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

        /* 
         * Registers the background task for this application. It uses a time trigger to
         * set the background task of every 15 minutes. Which should then give us a notification
         * if we are close to any existing room.
         */
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

        /* 
         * Request access for the background task, before conducting business.
         */
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Frame.BackStack.Clear();
        }
    }

}


