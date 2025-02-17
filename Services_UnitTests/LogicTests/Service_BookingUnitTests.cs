using DataHolder.Data;
using DataHolder.Data.DbModels;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Service_HotelManagement.Logic;
using System.Collections.ObjectModel;
using Services_UnitTests.Samples.LogicTestsSamples;
using Service_HotelManagement.Interfaces;

namespace Services_UnitTests.LogicTests
{
    public class Service_BookingUnitTests
    {
        private DbDataContext _mockContext;
        private IBookingService _bookingService;

        [SetUp]
        public void Setup()
        {
            _mockContext = PrepareMockData.ReturnMockDbDataContext();
            _bookingService = new BookingService(_mockContext);

        }

        [TearDown]
        public void TearDown()
        {
            _mockContext.Database.EnsureDeleted();
            _mockContext.Dispose();
        }

        [Test]
        public async Task ServiceShouldReturnBookings()
        {
            var result = await _bookingService.GetAllAsync();

            result.Count().Should().Be(1); 
        }

        [Test]
        public async Task ServiceShouldAddNewBooking()
        {
            var booking = new Booking
            {
                Name = "New booking",
                IsDeleted = false,
            };
            await _bookingService.AddAsync(booking);
            var result = await _bookingService.GetAllAsync();

            result.Should().Contain(b => b.Name == "New booking");
            result.Count().Should().Be(2);
        }

        [Test]
        public async Task ServiceShouldUpdateBooking()
        {
            var newBooking = new Booking
            {
                Name = "Before"
            };
            var x = await _bookingService.AddAsync(newBooking);

            var bookingToEdit = await _mockContext.Bookings.FirstAsync(r => r.Name.Contains("Before"));

            var booking = new Booking
            {
                Name = "After"
            };

            await _bookingService.UpdateAsync(bookingToEdit.Id, booking);

            var result = await _bookingService.GetAllAsync();

            result.Should().Contain(b => b.Name == "After");
            result.Should().NotContain(b => b.Name == "Before");
        }

        [Test]
        public async Task ServiceShouldGetBookingById()
        {
            await _mockContext.Bookings.AddAsync(new Booking
            {
                Id = 230,
                Name = "Found",
                IsDeleted = false
            });
            await _mockContext.SaveChangesAsync();

            var result = await _bookingService.GetByIdAsync(230);

            result.Name.Should().Be("Found");
        }

        [Test]
        public async Task ServiceShouldDeleteBooking()
        {
            await _mockContext.Bookings.AddAsync(new Booking
            {
                Id = 25,
                Name = "Before",
                IsDeleted = false
            });
            await _mockContext.SaveChangesAsync();

            var bookingToDelete = await _mockContext.Bookings.FirstAsync(b => b.Id == 25);
            await _bookingService.DeleteAsync(bookingToDelete.Id);

            var result = await _bookingService.GetAllAsync();
            result.Count().Should().Be(1);
            _mockContext.Bookings.Count().Should().Be(2); //Sanity check. Booking should be added but not visible due to IsDeleted
        }

        [Test]
        public async Task ServiceShouldThrowOperationIfBookingDoesNotExist() // Throw is handled in controller
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _bookingService.DeleteAsync(25));
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _bookingService.GetByIdAsync(25));
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _bookingService.UpdateAsync(25, new Booking()));
        }

        [Test]
        public async Task ServiceShouldSuccessfulyBookWhenClientExistsInDb()
        {
            var user = _mockContext.Users.First();
            var room = _mockContext.Rooms.First(r => r.RoomStatusDetailsPair.RoomStatus.Id == 1);

            var result = await _bookingService.BookRoomAsync(room.Id, user);

            result.Should().BeTrue();
            _mockContext.Bookings.Should().Contain(b => b.User.Surname.Equals("Tester"));
            _mockContext.Bookings.Should().Contain(b => b.Room.RoomIdentification.Equals("001"));
            _mockContext.Bookings.Should().Contain(b => b.Room.RoomStatusDetailsPair.RoomStatus.Id == 2);
        }

        [Test]
        public async Task ServiceShouldSuccessfulyBookWhenClientDoesNotExistsInDbYet()
        {
            var user = new User
            {
                Name = "New",
                Surname = "User",
                Email = "e@mail.net"

            };
            var room = _mockContext.Rooms.First(r => r.RoomStatusDetailsPair.RoomStatus.Id == 1);

            var result = await _bookingService.BookRoomAsync(room.Id, user);
            result.Should().BeTrue();
            _mockContext.Bookings.Should().Contain(b => b.User.Surname.Equals("User"));
            _mockContext.Bookings.Should().Contain(b => b.Room.RoomIdentification.Equals("001"));
            _mockContext.Bookings.Should().Contain(b => b.Room.RoomStatusDetailsPair.RoomStatus.Id == 2);
            _mockContext.Users.Count().Should().Be(2);
        }

        [Test]
        public async Task ServiceShouldSuccessfulyCheckOut()
        {
            var user = new User
            {
                Name = "New",
                Surname = "User",
                Email = "e@mail.net"

            };
            var room = _mockContext.Rooms.First(r => r.RoomStatusDetailsPair.RoomStatus.Id == 1);

            var bookingResult = await _bookingService.BookRoomAsync(room.Id, user);
            var checkingOutResult = await _bookingService.CheckOutRoomAsync(room.Id);

            bookingResult.Should().BeTrue();
            checkingOutResult.Should().BeTrue();
            _mockContext.Bookings.Count().Should().Be(2);
            _mockContext.Bookings.Should().Contain(b => b.IsDeleted // Is deleted
            && b.User.Name.Equals("New") // Was booked correctly
            && room.RoomStatusDetailsPair.RoomStatus.Id == 3); // Is marked as to be cleaned
        }
    }
}