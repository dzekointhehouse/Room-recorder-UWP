using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;
using Windows.Storage;
namespace BackgroundTask
{

        public sealed class Class1 : IBackgroundTask
        {
            private CancellationTokenSource _cts = null;

            //        ToastVisual visual = new ToastVisual()
            //        {
            //            BindingGeneric = new ToastBindingGeneric()
            //            {
            //                Children =
            //            {
            //            new AdaptiveText()
            //            {
            //            Text = title
            //            },

            //            new AdaptiveText()
            //            {
            //            Text = content
            //            },

            //new AdaptiveImage()
            //{
            //Source = image
            //}
            //},

            //                AppLogoOverride = new ToastGenericAppLogo()
            //                {
            //                    Source = logo,
            //                    HintCrop = ToastGenericAppLogoCrop.Circle
            //                }
            //            }
            //        };

            async void IBackgroundTask.Run(IBackgroundTaskInstance taskInstance)
            {

                BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

                try

                {

                    // Associate a cancellation handler with the background task.

                    taskInstance.Canceled += OnCanceled;

                    // Get cancellation token

                    if (_cts == null)

                    {
                        _cts = new CancellationTokenSource();
                    }

                    CancellationToken token = _cts.Token;
                    // Create geolocator object

                    Geolocator geolocator = new Geolocator();

                    // Make the request for the current position

                    Geoposition pos = await geolocator.GetGeopositionAsync().AsTask(token);

                    DateTime currentTime = DateTime.Now;

                    WriteStatusToAppData("Time: " + currentTime.ToString());

                    WriteGeolocToAppData(pos);

                }
                catch (UnauthorizedAccessException)
                {
                    WriteStatusToAppData("Disabled");
                    WipeGeolocDataFromAppData();
                }
                catch (Exception ex)
                {
                    WriteStatusToAppData(ex.ToString());
                    WipeGeolocDataFromAppData();

                }

                finally
                {

                    _cts = null;

                    deferral.Complete();

                }

            }

            private async void WriteGeolocToAppData(Geoposition pos)

            {

                //double Lat, Long, LatDiff, LongDiff;
                //var currentLat = Math.Abs(pos.Coordinate.Point.Position.Latitude);
                //var currentLong = Math.Abs(pos.Coordinate.Point.Position.Longitude);
                //var rooms = DatabaseRepository.GetRooms();

                //foreach (var item in rooms)
                //{
                //    Lat = Math.Abs(item.Lat);
                //    Long = Math.Abs(item.Longt);
                //    LongDiff = currentLong - Long;
                //    LatDiff = currentLat - Lat;
                //    if ((LatDiff < 5 && LatDiff > -5) && (LongDiff < 5 && LongDiff > -5))
                //    {
                //notifiera
                var dialog = new Windows.UI.Popups.MessageDialog("Du är nära ett rum");
                await dialog.ShowAsync();
                //}
                //}

            }

            private void WipeGeolocDataFromAppData()

            {

                var settings = ApplicationData.Current.LocalSettings;

                settings.Values["Latitude"] = "";

                settings.Values["Longitude"] = "";

                settings.Values["Accuracy"] = "";

            }



            private void WriteStatusToAppData(string status)

            {

                var settings = ApplicationData.Current.LocalSettings;

                settings.Values["Status"] = status;

            }



            private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)

            {

                if (_cts != null)

                {

                    _cts.Cancel();

                    _cts = null;

                }

            }

        }
    }

