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

        public static Room CreateRoom(string title, string description, double volume, double lat, double longt)
        {
            using (var db = new RoomDbContext())
            {
                if (db.Rooms.FirstOrDefault(g => g.Title == title) == null)
                {
                    Room currentroom = new Room
                    {
                        Title = title,
                        Description = description,
                        Lat = lat,
                        Longt = longt,
                        Volume = volume,
                        Surfaces = new List<Surface>()
                        {
                            new Surface(),
                            new Surface(),
                            new Surface(),
                            new Surface(),
                            new Surface(),
                            new Surface()
                        }
                    };
                    db.Rooms.Add(currentroom);
                    foreach (var surf in currentroom.Surfaces)
                    {
                        db.Surfaces.Add(surf);
                    }
                    db.SaveChanges();

                    // Return the new room
                    return GetRoom(currentroom.Title);
                }
                else
                {
                    // No new room created.
                    return null;
                }
            }
        }

        public static Room GetRoom(string title)
        {
            using (var db = new RoomDbContext())
            {
                try
                {
                    Room room = db.Rooms.Where(b => b.Title == title)
                        .Include(b => b.Surfaces)
                        .FirstOrDefault();

                    return room;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static void UpdateSurface(Surface currentSurface)
        {
            using (var db = new RoomDbContext())
            {
                try
                {
                    Surface surface = db.Surfaces.Find(currentSurface.SurfaceId);
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

        public static void UpdateRoom(Room room)
        {
            using (var db = new RoomDbContext())
            {
                try
                {
                    // Tror inte denna funkar riktigt
                    Room dbRoom = db.Rooms.Find(room.RoomId);
                    int i=0;
                    foreach (var surf in room.Surfaces)
                    {
                        dbRoom.Surfaces[i] = surf;
                        i++;
                    }
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

