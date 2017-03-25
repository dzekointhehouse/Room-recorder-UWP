using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using SQLite.Net.Attributes;

namespace CodebustersAppWMU3.Models
{
    public class Surface
    {
        public int SurfaceId { get; set; }
        public byte[] SurfaceImage { get; set; } = null;
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        // public double Area { get; set; }
    }
}
