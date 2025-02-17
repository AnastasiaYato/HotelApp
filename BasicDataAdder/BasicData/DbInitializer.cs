using DataHolder.Data.DbModels;
using DataHolder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDataAdder.BasicData
{
    /// <summary>
    /// Sample data initalizer for Prod and Reg. Prod will use its own seperate data but they will keep the same structure.
    /// (And in our case now - the same data, this can be changed by extending the logic - however as it's a demo I think it's better this way atm)
    /// </summary>
    public static class DbInitializer
    {
        public static void Initialize(DbDataContext _context)
        {
            try
            {
                _context.Database.EnsureDeleted();
                Console.WriteLine("Database deleted if it existed");

                _context.Database.EnsureCreated();
                Console.WriteLine("Database created");

                // Adding basic sample data
                AddPaymentMethods(_context);
                AddUsers(_context);
                AddRoomSizes(_context);
                AddRoomStatuses(_context);
                // Add Rooms
                AddSampleRooms(_context);
                AddBookings(_context);

                Console.WriteLine("Database is ready to use! Press any key to Exit");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }
        }

        private static void AddPaymentMethods(DbDataContext _context)
        {
            var paymentMethod = new PaymentMethod
            {
                Name = "Cash",
                Description = "Euro or Pln - current exchange ratio is on our website"
            };

            _context.PaymentMethods.Add(paymentMethod);
            Console.WriteLine("Added Payment Method: Cash");
        }

        private static void AddUsers(DbDataContext _context)
        {
            var users = new[]
            {
            new User { Name = "Jane", Surname = "Doe", Email = "j@d.pl", PhoneNo = "48111222333" },
            new User { Name = "Bob", Surname = "Marley", Email = "bob@reggae.pl", PhoneNo = "119451981" }
        };

            _context.Users.AddRange(users);
            Console.WriteLine("Added Sample Users");
        }

        private static void AddRoomSizes(DbDataContext _context)
        {
            var roomSizes = RoomsRelatedData.ReturnRoomSizes();
            _context.RoomSizes.AddRange(roomSizes);
            Console.WriteLine("Added Room Sizes");
        }

        private static void AddRoomStatuses(DbDataContext _context)
        {
            var roomStatuses = RoomsRelatedData.ReturnRoomStatuses();
            _context.RoomStatuses.AddRange(roomStatuses);
            _context.SaveChanges();
            Console.WriteLine("Added Room Statuses");
        }

        private static void AddSampleRooms(DbDataContext _context)
        {
            var rooms = RoomsRelatedData.ReturnSampleRooms(_context);
            _context.Rooms.AddRange(rooms);
            _context.SaveChanges();
            Console.WriteLine("Added Sample Rooms");
        }

        private static void AddBookings(DbDataContext _context)
        {
            _context.Bookings.Add(new Booking
            {
                Room = _context.Rooms.First(r => r.RoomStatusDetailsPair.RoomStatus.Id == 2),
                User = _context.Users.First()
            });
            _context.SaveChanges();
        }
    }
}
