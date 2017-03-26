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
        // For Existing rooms page
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

        public static Room CreateRoom(Room currentRoom)
        {
            using (var db = new RoomDbContext())
            {
                if (db.Rooms.FirstOrDefault(g => g.Title == currentRoom.Title) == null)
                {
                    db.Rooms.Add(currentRoom);
                    foreach (var surface in currentRoom.Surfaces)
                    {
                        db.Surfaces.Add(surface);
                    }
                    db.SaveChanges();

                    // Return the updated room
                    return GetRoom(currentRoom);
                }
                else
                {
                    // No new room created.
                    return null;
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

        //public static void SaveRoomSurface(string title, int surface, Surface side)
        //{
        //    using (var db = new RoomDbContext())
        //    {
        //        var room = (from r in db.Rooms
        //            where r.Title.Equals(title)
        //            select r).FirstOrDefault();

        //        room.Surfaces[surface] = side;

        //        db.Update(room);
        //        db.SaveChanges();
        //    }
        //}

        //public static Surface GetSurface(Room currentRoom, string surfaceTitle)
        //{
        //    using (var db = new RoomDbContext())
        //    {
        //        return (from room in db.Rooms
        //            where room.Title.Equals(currentRoom.Title)
        //            select room.Surfaces.FirstOrDefault(s => s.Title == surfaceTitle)).FirstOrDefault();
        //    }
        //}

        //public static void UpdateRoom(Room currentRoom)
        //{
        //    using (var db = new RoomDbContext())
        //    {
        //        db.Update(currentRoom);
        //        db.SaveChanges();
        //    }
        //}

        public static void UpdateSurface(Surface currentSurface)
        {
            using (var db = new RoomDbContext())
            {
                try
                {
                    var surface = db.Surfaces.Find(currentSurface.SurfaceId);
                    surface.Title = currentSurface.Title;
                    surface.Description = currentSurface.Description;
                    surface.SurfaceImage = currentSurface.SurfaceImage;
                    db.SaveChanges();
                }
                catch
                {
                    ErrorMessage.DisplayErrorDialog("Could not Update!");
                }
            }
        }
    }
}

