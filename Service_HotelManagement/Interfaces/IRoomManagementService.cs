using DataHolder.Data.DbModels;
using Service_HotelManagement.Interfaces.Shared;

namespace Service_HotelManagement.Interfaces
{
    /// <summary>
    /// Interface for the room management service.
    /// </summary>
    public interface IRoomManagementService : IBasicService<Room>
    {
        Task<bool> ChangeStatusAsync(int id, RoomStatusDetailsPair roomStatus);
        Task<IEnumerable<Room>> GetAllAsync(string? name = null, int? roomSizeId = null, bool? isAvailable = null);
    }
}
