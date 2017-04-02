using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace CodebustersAppWMU3.Services
{
    class LocationService
    {
        private Geolocator locator;
        public LocationService()
        {
            locator = new Geolocator();
            locator.DesiredAccuracyInMeters = 1;
        }
        public async Task<Geoposition> GetPosition()
        {
            var position = await locator.GetGeopositionAsync();
            return position;
        }

    }

}
