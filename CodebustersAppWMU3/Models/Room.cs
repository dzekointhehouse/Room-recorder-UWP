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
        public double Longt { get; set; }
        public double Lat { get; set; }
        public List<Surface> Surfaces { get; set; } = new List<Surface>();
        public double Volume { get; set; }   
    }
}
