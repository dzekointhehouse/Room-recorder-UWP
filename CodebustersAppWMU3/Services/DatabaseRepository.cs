using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodebustersAppWMU3.Models;

namespace CodebustersAppWMU3.Services
{
    class DatabaseRepository
    {
        public static Room GetRoom(string title)
        {
            using (var db = new RoomDbContext())
            {
                return (from room in db.Rooms
                        where room.Title.Equals(title)
                        select room).FirstOrDefault();
            }
        }

        public static void SaveRoomSurface(string title, int surface, Surface side)
        {
            using (var db = new RoomDbContext())
            {
                var room = (from r in db.Rooms
                            where r.Title.Equals(title)
                            select r).FirstOrDefault();

                room.Surfaces[surface] = side;

                db.Attach(room);
                db.Update(room);
                db.SaveChanges();
            }
        }
    }
}
