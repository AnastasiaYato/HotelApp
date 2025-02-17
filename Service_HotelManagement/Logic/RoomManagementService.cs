using DataHolder.Data;
using DataHolder.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using Service_HotelManagement.Interfaces;
using Service_HotelManagement.Logic.Shared;

namespace Service_HotelManagement.Logic
{
    /// <summary>
    /// Our room management service!
    /// </summary>
    public class RoomManagementService : BasicService, IRoomManagementService
    {
        public RoomManagementService(DbDataContext context) : base(context)
        {
        }

        public async Task<bool> ChangeStatusAsync(int id, RoomStatusDetailsPair roomStatus) 
        {
            if (RoomIsBooked(id)) return false; // We don't want to change status of a room that is booked
            if (roomStatus == null || roomStatus.RoomStatus.Id == 0) return false;
            if (roomStatus.RoomStatusDetails == null && roomStatus.RoomStatus.Id > 2) return false; // yet another validation
            var roomToEdit = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            roomToEdit.RoomStatusDetailsPair.RoomStatus = await _context.RoomStatuses.FirstOrDefaultAsync(rs => rs.Id == roomStatus.RoomStatus.Id);
            roomToEdit.RoomStatusDetailsPair.RoomStatusDetails = roomStatus.RoomStatusDetails;
            await _context.SaveChangesAsync();
            return true;

        }
        public async Task<Room> AddAsync(Room entity)
        {
            var roomToAdd = new Room() // Generally it will throw if we lack some details - it is handled in the controller
            {
                Name = entity.Name,
                Description = entity.Description,
                RoomIdentification = entity.RoomIdentification,
                FloorNo = entity.FloorNo,
                Price = entity.Price,
                RoomSize = await _context.RoomSizes.FirstOrDefaultAsync(rs => rs.Id == entity.RoomSize.Id),
                RoomStatusDetailsPair = new RoomStatusDetailsPair
                {
                    RoomStatus = await _context.RoomStatuses.FirstOrDefaultAsync(rs => rs.Id == entity.RoomStatusDetailsPair.RoomStatus.Id),
                    RoomStatusDetails = entity.RoomStatusDetailsPair.RoomStatusDetails
                },
                CreatedOn = DateTime.UtcNow
            };
            _context.Rooms.Add(roomToAdd);
            await _context.SaveChangesAsync();
            return roomToAdd;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var roomToDelete = await GetByIdAsync(id);
            if (roomToDelete == null) return false;
            roomToDelete.IsDeleted = true;
            roomToDelete.DeletedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Room>> GetAllAsync(string? name = null, int? roomSizeId = null, bool? isAvailable = null)
        {
            var filter = _context.Rooms.Where(b => !b.IsDeleted);

            if (!string.IsNullOrEmpty(name))
            {
                string lowerName = name.ToLower();
                filter = filter.Where(r => r.Name.ToLower().Contains(lowerName));
            }

            if (roomSizeId.HasValue) filter = filter.Where(r => r.RoomSize.Id == roomSizeId.Value);

            if (isAvailable.HasValue)
                if (isAvailable.Value) filter = filter.Where(r => r.RoomStatusDetailsPair.RoomStatus.Id == 1);
                else filter = filter.Where(r => r.RoomStatusDetailsPair.RoomStatus.Id != 1);

            return await filter.ToListAsync();
        }
        public async Task<Room> GetByIdAsync(int id)
        {
            return await _context.Rooms.Where(b => b.Id == id && !b.IsDeleted).FirstAsync();
        }

        public async Task<Room> UpdateAsync(int id, Room entity)
        {
            var existingRoom = await _context.Rooms.FindAsync(id);
            if (existingRoom == null) throw new InvalidOperationException();
            if (await ChangeStatusAsync(id, entity.RoomStatusDetailsPair)) // The same case as with AddAsync
            {
                existingRoom.Name = entity.Name;
                existingRoom.Description = entity.Description;
                existingRoom.RoomIdentification = entity.RoomIdentification;
                existingRoom.IsDeleted = entity.IsDeleted;
                existingRoom.FloorNo = entity.FloorNo;
                existingRoom.Price = entity.Price;
                existingRoom.RoomSize.Id = entity.RoomSize.Id;
                existingRoom.UpdatedOn = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            else return null;
            return existingRoom;
        }

        public Task<IEnumerable<Room>> GetAllAsync()
        {
            return GetAllAsync(null, null, null);
        }
        private bool RoomIsBooked(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Room.Id == id && !b.IsDeleted);
            return booking!=null;
        }
    }
}
