using DataHolder.Data.DbModels;
using Service_HotelManagement.Interfaces.Shared;

namespace Service_HotelManagement.Interfaces
{
    /// <summary>
    /// Interface for the booking service.
    /// </summary>
    public interface IBookingService : IBasicService<Booking>
    {
        Task<bool> BookRoomAsync(int id, User user);
        Task<bool> CheckOutRoomAsync(int id);
    }
}
