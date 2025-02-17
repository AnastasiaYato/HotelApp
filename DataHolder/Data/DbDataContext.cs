using DataHolder.Data.DbModels;
using DataHolder.Data.Health;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHolder.Data
{
    public class DbDataContext : DbContext
    {
        private readonly DbContextOptions<DbDataContext> _options;
        public DbContextOptions<DbDataContext> Options
        {
            get
            {
                return _options;
            }
        }
        public DbDataContext(DbContextOptions<DbDataContext> options)
            : base(options)
        {
            _options = options;
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomSize> RoomSizes { get; set; }
        public DbSet<RoomStatus> RoomStatuses { get; set; }
        public DbSet<RoomStatusDetails> RoomStatusDetails { get; set; }
        public DbSet<RoomStatusDetailsPair> RoomStatusDetailsPairs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
            .HasOne(r => r.RoomSize)
            .WithMany()
            .HasForeignKey(r => r.RoomSizeId);
            modelBuilder.Entity<RoomStatusDetailsPair>()
                    .HasOne(r => r.RoomStatus)
                    .WithMany()
                    .HasForeignKey(r => r.RoomStatusId);
            modelBuilder.Entity<RoomStatusDetailsPair>()
                .HasOne(r => r.RoomStatusDetails)
                .WithOne(r => r.RoomStatusDetailsPair)
                .HasForeignKey<RoomStatusDetailsPair>(r => r.RoomStatusDetailsId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
//ANL2025 