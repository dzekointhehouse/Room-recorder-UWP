using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodebustersAppWMU3.Models
{
    class Room
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public List<Surface> Surfaces { get; set; }
        public double Volume { get; set; }   
    }
}
