using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CodebustersAppWMU3.Models;

namespace CodebustersAppWMU3.Migrations
{
    [DbContext(typeof(RoomDbContext))]
    partial class RoomDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("CodebustersAppWMU3.Models.Room", b =>
                {
                    b.Property<int>("RoomId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<double>("Lat");

                    b.Property<double>("Longt");

                    b.Property<string>("Title");

                    b.Property<double>("Volume");

                    b.HasKey("RoomId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("CodebustersAppWMU3.Models.Surface", b =>
                {
                    b.Property<int>("SurfaceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int?>("RoomId");

                    b.Property<byte[]>("SurfaceImage");

                    b.Property<string>("Title");

                    b.HasKey("SurfaceId");

                    b.HasIndex("RoomId");

                    b.ToTable("Surfaces");
                });

            modelBuilder.Entity("CodebustersAppWMU3.Models.Surface", b =>
                {
                    b.HasOne("CodebustersAppWMU3.Models.Room")
                        .WithMany("Surfaces")
                        .HasForeignKey("RoomId");
                });
        }
    }
}
