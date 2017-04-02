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
using Windows.UI.Notifications;

namespace BackgroundTask
{

    public sealed class RoomSensorTask : IBackgroundTask
    {

        void IBackgroundTask.Run(IBackgroundTaskInstance taskInstance)
        {
            //BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            ToastNotifier();

        }

        void ToastNotifier() {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
            Windows.Data.Xml.Dom.XmlDocument toastxml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            Windows.Data.Xml.Dom.XmlNodeList toastTextElements = toastxml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastxml.CreateTextNode("You are near a room"));
            ToastNotification toast = new ToastNotification(toastxml);

            ToastNotificationManager.CreateToastNotifier().Show(toast);

        }



    }

        //private async void WriteGeolocToAppData(Geoposition pos)

        //{

        //    //double Lat, Long, LatDiff, LongDiff;
        //    //var currentLat = Math.Abs(pos.Coordinate.Point.Position.Latitude);
        //    //var currentLong = Math.Abs(pos.Coordinate.Point.Position.Longitude);
        //    //var rooms = DatabaseRepository.GetRooms();

        //    //foreach (var item in rooms)
        //    //{
        //    //    Lat = Math.Abs(item.Lat);
        //    //    Long = Math.Abs(item.Longt);
        //    //    LongDiff = currentLong - Long;
        //    //    LatDiff = currentLat - Lat;
        //    //    if ((LatDiff < 5 && LatDiff > -5) && (LongDiff < 5 && LongDiff > -5))
        //    //    {
        //    //notifiera
        //    var dialog = new Windows.UI.Popups.MessageDialog("Du är nära ett rum");
        //    await dialog.ShowAsync();
        //    //}
        //    //}

        //}

     

    }
    

