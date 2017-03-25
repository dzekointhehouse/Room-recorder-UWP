using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace CodebustersAppWMU3.Models
{
    public class Room
    {

        public int RoomId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Longt { get; set; }
        public double Lat { get; set; }
        public List<Surface> Surfaces { get; set; } = new List<Surface> { new Surface(), new Surface(), new Surface(), new Surface(), new Surface(), new Surface()};
        public double Volume { get; set; } = 0;

    }
}