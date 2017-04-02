using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Notifications;
using CodebustersAppWMU3.Services;

namespace BackgroundTask
{
    /* 
     * Unfortunately we use a bad practice here by copy and creating redundant 
     * number of classes that are exactly the same. If we would have more time on this
     * project, and if we were brieft in a better way early on then we would have done 
     * this a little bit differently. Communication with the database should have been
     * an separate project for reusability.
     */
    public sealed class RoomSensorTask : IBackgroundTask
    {

        void IBackgroundTask.Run(IBackgroundTaskInstance taskInstance)
        {
            //BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            ToastNotifier();

        }

        void ToastNotifier()
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
            Windows.Data.Xml.Dom.XmlDocument toastxml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            Windows.Data.Xml.Dom.XmlNodeList toastTextElements = toastxml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastxml.CreateTextNode("You are near a room"));
            ToastNotification toast = new ToastNotification(toastxml);

            ToastNotificationManager.CreateToastNotifier().Show(toast);

        }



        private async void CheckWithDatabase(Geoposition pos)

        {
            Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 50 };

            // Subscribe to the PositionChanged event to get location updates.
            var position = await geolocator.GetGeopositionAsync();
            var myposition = position.Coordinate.Point;

            double Lat, Long, LatDiff, LongDiff;
            var currentLat = Math.Abs(pos.Coordinate.Point.Position.Latitude);
            var currentLong = Math.Abs(pos.Coordinate.Point.Position.Longitude);
            var rooms = DatabaseRepository.GetRooms();

            foreach (var item in rooms)
            {
                Lat = Math.Abs(item.Lat);
                Long = Math.Abs(item.Longt);
                LongDiff = currentLong - Long;
                LatDiff = currentLat - Lat;
                if ((LatDiff < 5 && LatDiff > -5) && (LongDiff < 5 && LongDiff > -5))
                {
                    // Notifies the user if a existing room is close to phones location
                    ToastNotifier();
                }
            }

        }

    }
}


