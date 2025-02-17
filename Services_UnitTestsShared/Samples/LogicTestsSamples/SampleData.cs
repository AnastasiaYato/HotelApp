using DataHolder.Data;
using DataHolder.Data.DbModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_UnitTests.Samples.LogicTestsSamples
{
    public static class SampleData
    {
        public static ICollection<RoomStatus> RoomStatuses()
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
        public static ICollection<RoomSize> RoomSizes()
        {
            return new List<RoomSize>()
            {
                new RoomSize { Name = "Small", Description = "Small but cozy!" },
                new RoomSize { Name = "Medium", Description = "Standard size" },
                new RoomSize { Name = "Large", Description = "Spacious, you can feel like a king in a castle" },
            };
        }
        public static ICollection<Room> Rooms(DbDataContext context)
        {

            return new List<Room>()
            {
                new Room {
                    RoomIdentification = "001",
                    FloorNo = 0,
                    Price = 240.99,
                    Name = "Cozy Room",
                    Description = "1 bed, AC, Free WiFi",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair{RoomStatus = context.RoomStatuses.First(x => x.Id == 1) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 1)
                },
                new Room {
                    RoomIdentification = "506",
                    FloorNo = 5,
                    Price = 21.37,
                    Name = "Nice views included",
                    Description = "2 beds, AC, Free WiFi",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair{RoomStatus = context.RoomStatuses.First(x => x.Id == 2) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 2)
                },
                new Room {
                    RoomIdentification = "104",
                    FloorNo = 1,
                    Price = 720.50,
                    Name = "Nice views included",
                    Description = "2 beds, AC, Free WiFi",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = context.RoomStatuses.First(x => x.Id == 3) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 3)
                },
                new Room {
                    RoomIdentification = "105",
                    FloorNo = 1,
                    Price = 420.69,
                    Name = "Nice views included",
                    Description = "2 beds, AC, Free WiFi",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = context.RoomStatuses.First(x => x.Id == 4) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 1)
                },
                new Room {
                    RoomIdentification = "306",
                    FloorNo = 3,
                    Price = 220.50,
                    Name = "Premium",
                    Description = "1 big bed, AC, Free WiFi",
                    RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = context.RoomStatuses.First(x => x.Id == 5) },
                    RoomSize = context.RoomSizes.First(x => x.Id == 3)
                }
            };

        }
    }
}
