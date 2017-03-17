using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace CodebustersAppWMU3.Models
{
    class Surface
    {
        public StorageFile SurfaceImage { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        // public double Area { get; set; }
    }
}
