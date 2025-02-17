using DataHolder.Data;
using DataHolder.Data.DbModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDataAdder.BasicData
{
    /// <summary>
    /// Simple sample data return to keep it in one place
    /// </summary>
    public static class RoomsRelatedData
    {
        public static ICollection<Room> ReturnSampleRooms(DbDataContext context)
        {
            return new List<Room>
            {
                new Room
                {
                    RoomIdentification = "001",
                    FloorNo = 0,
                    Price = 240.99,
                    Name = "Cozy Room",
                    Description = "1 bed, AC, Free WiFi",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = context.RoomStatuses.First(x => x.Id == 1) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 1)
                },
                new Room
                {
                    RoomIdentification = "002",
                    FloorNo = 0,
                    Price = 300.50,
                    Name = "Luxury Room",
                    Description = "1 bed, AC, Free WiFi, Mini Bar",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = context.RoomStatuses.First(x => x.Id == 2) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 2)
                },
                new Room
                {
                    RoomIdentification = "103",
                    FloorNo = 1,
                    Price = 150.75,
                    Name = "Economy Room",
                    Description = "1 bed, AC, Free WiFi",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = context.RoomStatuses.First(x => x.Id == 1) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 1)
                },
                new Room
                {
                    RoomIdentification = "104",
                    FloorNo = 1,
                    Price = 450.00,
                    Name = "Premium Suite",
                    Description = "2 beds, AC, Free WiFi, Ocean View",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = context.RoomStatuses.First(x => x.Id == 2) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 3)
                },
                new Room
                {
                    RoomIdentification = "205",
                    FloorNo = 2,
                    Price = 200.99,
                    Name = "Standard Room",
                    Description = "1 bed, AC, Free WiFi",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = context.RoomStatuses.First(x => x.Id == 1) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 1)
                },
                new Room
                {
                    RoomIdentification = "306",
                    FloorNo = 3,
                    Price = 550.99,
                    Name = "Executive Suite",
                    Description = "2 beds, AC, Free WiFi, Mini Bar, Ocean View",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = context.RoomStatuses.First(x => x.Id == 2) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 3)
                },
                new Room
                {
                    RoomIdentification = "407",
                    FloorNo = 4,
                    Price = 320.00,
                    Name = "Business Room",
                    Description = "1 bed, AC, Free WiFi, Work Desk",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = context.RoomStatuses.First(x => x.Id == 1) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 2)
                },
                new Room
                {
                    RoomIdentification = "508",
                    FloorNo = 5,
                    Price = 680.50,
                    Name = "Presidential Suite",
                    Description = "3 beds, AC, Free WiFi, Mini Bar, Ocean View, Jacuzzi",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = context.RoomStatuses.First(x => x.Id == 2) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 3)
                },
                new Room
                {
                    RoomIdentification = "109",
                    FloorNo = 1,
                    Price = 240.99,
                    Name = "Cozy Room",
                    Description = "1 bed, AC, Free WiFi",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = context.RoomStatuses.First(x => x.Id == 1) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 1)
                },
                new Room
                {
                    RoomIdentification = "210",
                    FloorNo = 2,
                    Price = 400.00,
                    Name = "Family Room",
                    Description = "2 beds, AC, Free WiFi, Kid Friendly",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = context.RoomStatuses.First(x => x.Id == 2) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 2)
                }
            };
        }
        public static ICollection<RoomStatus> ReturnRoomStatuses()
        {
            return new List<RoomStatus>()
            {
                new RoomStatus { Name = "Free", Description = "Room is ready to be booked" },
                new RoomStatus { Name = "Occupied", Description = "Room is taken" },
                new RoomStatus { Name = "During Cleaning", Description = "Room is being cleaned" },
                new RoomStatus { Name = "During Maintenance", Description = "Room is under maintenance" },
                new RoomStatus { Name = "Manually Locked", Description = "VIP - Premium" },
            };
        }
        public static ICollection<RoomSize> ReturnRoomSizes()
        {
            return new List<RoomSize>()
            {
                new RoomSize { Name = "Small", Description = "Small but cozy!" },
                new RoomSize { Name = "Medium", Description = "Standard size" },
                new RoomSize { Name = "Large", Description = "Spacious, you can feel like a king in a castle" },
            };
        }
    }
}
