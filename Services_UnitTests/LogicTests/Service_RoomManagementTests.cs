using DataHolder.Data;
using DataHolder.Data.DbModels;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Service_HotelManagement.Interfaces;
using Service_HotelManagement.Logic;
using Services_UnitTests.Samples.LogicTestsSamples;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Services_UnitTests.LogicTests
{
    public class Service_RoomManagementTests
    {
        private DbDataContext _mockContext;
        private IRoomManagementService _roomManagementService;

        [SetUp]
        public void Setup()
        {
            _mockContext = PrepareMockData.ReturnMockDbDataContext();
            _roomManagementService = new RoomManagementService(_mockContext);
        }

        [TearDown]
        public void TearDown()
        {
            _mockContext.Database.EnsureDeleted();
            _mockContext.Dispose();
        }

        [Test]
        public async Task ServiceShouldReturnAllRooms()
        {
            var result = await _roomManagementService.GetAllAsync();

            result.Count().Should().Be(5);
        }

        [Test]
        public async Task ServiceShouldFilterResultsByName()
        {
            var nameToFind = "Premium";
            var results = await _roomManagementService.GetAllAsync(nameToFind);

            results.Should().NotBeEmpty();
            results.Should().HaveCount(1);
        }

        [Test]
        public async Task ServiceShouldFilterResultsBySize()
        {
            var sizeToFind = await _mockContext.RoomSizes.FirstAsync(rs => rs.Name.Equals("Medium"));
            var results = await _roomManagementService.GetAllAsync(null, sizeToFind.Id);

            results.Should().NotBeEmpty();
            results.Should().HaveCount(1);
        }

        [Test]
        public async Task ServiceShouldFilterResultsByAvailability()
        {
            var freeRooms = await _roomManagementService.GetAllAsync(isAvailable: true);
            var takenRooms = await _roomManagementService.GetAllAsync(isAvailable: false);

            freeRooms.Should().NotBeEmpty();
            takenRooms.Should().NotBeEmpty();
            freeRooms.Should().HaveCount(1);
            takenRooms.Should().HaveCount(4);
        }

        [Test]
        public async Task ServiceShouldAddNewRoom()
        {
            var newRoom = new Room()
            {
                Name = "TestRoom",
                RoomSize = _mockContext.RoomSizes.Find(1),
                RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = _mockContext.RoomStatuses.FirstOrDefault(x => x.Id == 1) },
                RoomIdentification = "901"
            };

            await _roomManagementService.AddAsync(newRoom);

            var result = await _roomManagementService.GetAllAsync();
            result.Count().Should().Be(6);
            result.First(x => x.Name.Equals("TestRoom")).RoomIdentification.Should().Be("901");
            result.First(x => x.Name.Equals("TestRoom")).RoomSize.Name.Should().Be("Small");
            result.First(x => x.Name.Equals("TestRoom")).RoomStatusDetailsPair.RoomStatus.Name.Should().Be("Free");
        }

        [Test]
        public async Task ServiceShouldUpdateRoom()
        {
            var freeRooms = await _roomManagementService.GetAllAsync(isAvailable: true);
            var roomToUpdate = freeRooms.First();
            var newRoom = new Room()
            {
                Name = "TestOfUpdate",
                RoomSize = roomToUpdate.RoomSize,
                RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = _mockContext.RoomStatuses.FirstOrDefault(x => x.Id == 2) }
            };

            await _roomManagementService.UpdateAsync(roomToUpdate.Id, newRoom);

            var result = await _roomManagementService.GetByIdAsync(1);
            result.Name.Should().Be("TestOfUpdate");
            result.RoomStatusDetailsPair.RoomStatus.Id.Should().Be(2);
            roomToUpdate.Id.Should().Be(result.Id);
        }

        [Test]
        public async Task ServiceShouldNotUpdateRoomIfDetailsAreNotProvided()
        {
            var freeRooms = await _roomManagementService.GetAllAsync(isAvailable: true);
            var roomToUpdate = freeRooms.First();
            var newRoom = new Room()
            {
                Name = "TestOfUpdate",
                RoomSize = roomToUpdate.RoomSize,
                RoomStatusDetailsPair = new RoomStatusDetailsPair { RoomStatus = _mockContext.RoomStatuses.FirstOrDefault(x => x.Id == 4) }
            };

            await _roomManagementService.UpdateAsync(roomToUpdate.Id, newRoom);

            var result = await _roomManagementService.GetByIdAsync(1);
            result.Name.Should().NotBe("TestOfUpdate");
            result.RoomStatusDetailsPair.RoomStatus.Id.Should().Be(1);
            roomToUpdate.Id.Should().Be(result.Id);
        }

        [Test]
        public async Task ServiceShouldDeleteRoom()
        {
            await _roomManagementService.DeleteAsync(1);

            var results = await _roomManagementService.GetAllAsync();
            results.Count().Should().Be(4);
            _mockContext.Rooms.Count().Should().Be(5); //Sanity check. Booking should be added but not visible due to IsDeleted
        }

        [Test]
        public async Task ServiceShouldThrowOperationIfRoomDoesNotExist() // Throw is handled in controller
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _roomManagementService.DeleteAsync(25));
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _roomManagementService.GetByIdAsync(25));
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _roomManagementService.UpdateAsync(25, new Room()));
        }

        [Test]
        public async Task ServiceShouldGetRoomById()
        {
            var result = await _roomManagementService.GetByIdAsync(2);
            result.Name.Should().Be("Nice views included");

        }

        [Test]
        public async Task ServiceShouldSuccessfulyChangeStatus()
        {
            var newStatus = await _mockContext.RoomStatuses.FindAsync(2);
            var roomToChange = await _roomManagementService.GetByIdAsync(1);
            var oldStatus = roomToChange.RoomStatusDetailsPair.RoomStatus;

            await _roomManagementService.ChangeStatusAsync(1, new RoomStatusDetailsPair { RoomStatus = _mockContext.RoomStatuses.FirstOrDefault(x => x.Id == 2) });
            var result = await _roomManagementService.GetByIdAsync(1);
            result.RoomStatusDetailsPair.RoomStatus.Id.Should().Be(2);
            result.RoomStatusDetailsPair.RoomStatus.Name.Should().Be("Occupied");
            oldStatus.Should().NotBe(result.RoomStatusDetailsPair.RoomStatus);
        }

        [Test]
        public async Task ServiceShouldNotChangeStatusToFreeIfRoomIsStillBooked()
        {
            var newStatus = await _mockContext.RoomStatuses.FindAsync(1);
            var roomToChange = await _roomManagementService.GetByIdAsync(2);
            var oldStatus = roomToChange.RoomStatusDetailsPair.RoomStatus;

            await _roomManagementService.ChangeStatusAsync(1, new RoomStatusDetailsPair { RoomStatus = _mockContext.RoomStatuses.FirstOrDefault(x => x.Id == 1) });
            var result = await _roomManagementService.GetByIdAsync(2);
            result.RoomStatusDetailsPair.RoomStatus.Id.Should().Be(2);
            result.RoomStatusDetailsPair.RoomStatus.Name.Should().Be("Occupied");
            oldStatus.Should().Be(result.RoomStatusDetailsPair.RoomStatus);
        }

        [Test]
        public async Task ServiceShouldNotChangeStatusWithoutReason()
        {
            var roomStatus = await _mockContext.RoomStatuses.FindAsync(4);
            var result = await _roomManagementService.ChangeStatusAsync(1, new RoomStatusDetailsPair { RoomStatus = _mockContext.RoomStatuses.FirstOrDefault(x => x.Id == 4) }); ;
            var sanityCheck = await _roomManagementService.GetByIdAsync(1);

            result.Should().BeFalse();
            sanityCheck.RoomStatusDetailsPair.RoomStatus.Id.Should().Be(1);
        }

        [Test]
        public async Task ServiceShouldChangeStatusWithReason()
        {
            var roomStatus = new RoomStatusDetailsPair()
            {
                RoomStatus = await _mockContext.RoomStatuses.FindAsync(4),
                RoomStatusDetails = new RoomStatusDetails
                {
                    Name = "Issue with AC",
                    Description = "AC is stuck on 20*C"
                }
            };

            var result = await _roomManagementService.ChangeStatusAsync(1, roomStatus);
            var sanityCheck = await _roomManagementService.GetByIdAsync(1);

            result.Should().BeTrue();
            sanityCheck.RoomStatusDetailsPair.RoomStatus.Id.Should().Be(4);
            sanityCheck.RoomStatusDetailsPair.RoomStatusDetails.Name.Should().Be("Issue with AC");
        }
    }
}