using DataHolder.Data;
using DataHolder.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using Service_HotelManagement.Interfaces;
using Service_HotelManagement.Logic.Shared;

namespace Service_HotelManagement.Logic
{
    /// <summary>
    /// Our booking service!
    /// </summary>
    public class BookingService : BasicService, IBookingService
    {

        public BookingService(DbDataContext context) : base(context)
        {

        }

        public async Task<bool> BookRoomAsync(int id, User user)
        {
            if (!await UserExists(user)) await CreateNewUser(user);
            var roomToBook = await _context.Rooms.FirstAsync(r => r.Id == id);
            var status = roomToBook.RoomStatusDetailsPair.RoomStatus.Id;
            if (roomToBook == null || status != 1) 
                return false; 

            var newBooking = new Booking()
            {
                User = user,
                Room = roomToBook
            };

            _context.Bookings.Add(newBooking);
            roomToBook.RoomStatusDetailsPair.RoomStatus = await _context.RoomStatuses.FirstAsync(rs => rs.Id == 2);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CheckOutRoomAsync(int id)
        {
            var roomToCheckOut = await _context.Rooms.FirstAsync(r => r.Id == id);
            var status = roomToCheckOut.RoomStatusDetailsPair.RoomStatus.Id;
            if (roomToCheckOut == null || status == 1) 
                return false; 
            var bookingToRemove = await _context.Bookings.FirstAsync(b => b.Room.Id == id);
            roomToCheckOut.RoomStatusDetailsPair.RoomStatus = await _context.RoomStatuses.FirstAsync(rs => rs.Id == 3); // Mark as to be cleaned
            bookingToRemove.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _context.Bookings.Where(b => !b.IsDeleted).ToListAsync();
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            return await _context.Bookings.Where(b => b.Id == id && !b.IsDeleted).FirstAsync();
        }
        
        public async Task<Booking> AddAsync(Booking entity) 
        {
            _context.Bookings.Add(entity); // Would be good to put some validation here
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Booking> UpdateAsync(int id, Booking entity)
        {
            var existingBooking = await _context.Bookings.FindAsync(id);
            if (existingBooking == null) throw new InvalidOperationException();

            existingBooking.Name = entity.Name;
            existingBooking.Description = entity.Description;
            existingBooking.IsDeleted = entity.IsDeleted;
            existingBooking.From = entity.From;
            existingBooking.To = entity.To;
            existingBooking.UpdatedOn = DateTime.UtcNow;
            try
            {
                existingBooking.User.Id = entity.User.Id;
                existingBooking.Room.Id = entity.Room.Id;
                existingBooking.PaymentMethod.Id = entity.PaymentMethod.Id;

            }
            catch (Exception ex)
            {
                return existingBooking;
            }
            finally
            {
                await _context.SaveChangesAsync();
            }
            return existingBooking;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bookingToDelete = await GetByIdAsync(id);
            if (bookingToDelete == null) return false;
            bookingToDelete.IsDeleted = true;
            bookingToDelete.DeletedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> UserExists(User user)
        {
            try
            {
                return await Task.Run(() =>
               _context.Users.Any(u =>
                   u.Name == user.Name &&
                   u.Surname == user.Surname &&
                   u.Email == user.Email &&
                   u.PhoneNo == user.PhoneNo));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task<bool> CreateNewUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
