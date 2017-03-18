using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using CodebustersAppWMU3.Models;
using Microsoft.EntityFrameworkCore;
using SQLite.Net;

namespace CodebustersAppWMU3.Models
{
    public class RoomDbContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Surface> Surfaces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=RoomDocumentation.db");
        }



    }

}
