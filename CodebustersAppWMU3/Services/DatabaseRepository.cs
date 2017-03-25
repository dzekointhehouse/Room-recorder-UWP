using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodebustersAppWMU3.Models;
using Microsoft.EntityFrameworkCore;

namespace CodebustersAppWMU3.Services
{
    class DatabaseRepository
    {
        public static Room GetRoom(string title)
        {
            using (var db = new RoomDbContext())
            {
                // navigate an association between two entity types, 
                // and loading the related Navigation properties is 
                // achieved by use of the Include method.
                return (from room in db.Rooms
                    where room.Title.Equals(title)
                    select room).Include(s => s.Surfaces).FirstOrDefault();
            }
        }


        public static string CreateRoom(Room currentRoom)
        {
            using (var db = new RoomDbContext())
            {
                if (db.Rooms.FirstOrDefault(g => g.Title == currentRoom.Title) == null)
                {
                    db.Rooms.Add(currentRoom);
                    db.SaveChanges();
                    return "Room Created";
                }
                else
                {
                    return "Room Already Exists";
                }
            }
        }

        public static Room GetRoom(Room currentRoom)
        {
            using (var db = new RoomDbContext())
            {
                // navigate an association between two entity types, 
                // and loading the related Navigation properties is 
                // achieved by use of the Include method.
                return (from room in db.Rooms
                    where room.Title.Equals(currentRoom.Title)
                    select room).Include(s => s.Surfaces).FirstOrDefault();
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

                db.Update(room);
                db.SaveChanges();
            }
        }

        public static Surface GetSurface(Room currentRoom, string surfaceTitle)
        {
            using (var db = new RoomDbContext())
            {
                return (from room in db.Rooms
                    where room.Title.Equals(currentRoom.Title)
                    select room.Surfaces.FirstOrDefault(s => s.Title == surfaceTitle)).FirstOrDefault();
            }
        }

        public static void UpdateRoom(Room currentRoom)
        {
            using (var db = new RoomDbContext())
            {
                db.Update(currentRoom);
                db.SaveChanges();
            }
        }

        public static void UpdateSurface(Room currentRoom, int surfaceIndex)
        {
            using (var db = new RoomDbContext())
            {
                db
                var d = db.Update(currentRoom.Surfaces[surfaceIndex]);
                db.SaveChanges();
            }
        }
    }
}

