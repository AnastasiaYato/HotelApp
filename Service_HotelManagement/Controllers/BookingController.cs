using DataHolder.Data.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service_HotelManagement.Controllers.Shared;
using Service_HotelManagement.Interfaces;

namespace Service_HotelManagement.Controllers
{
    /// <summary>
    /// We traded code readbility for code reusability. This controller is coming from a generic controller. Check the GenericController.cs file for more information.
    /// </summary>
    public class BookingController : GenericController<IBookingService, BookingController, Booking>
    {
        public BookingController(IBookingService bookingService, ILogger<BookingController> logger) : base(bookingService, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var bookings = await _service.GetAllAsync();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting all bookings");
                return BadRequest(ex);
            }
            
        }
        /// <summary>
        /// Books a room for a user. If user does not exist, it will create it in the database.
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("book/{roomId}")]
        public async Task<IActionResult> BookRoomAsync(int roomId, [FromBody] User user) 
        {
            try
            {
                if (user == null) return BadRequest();
                var updatedBooking = await _service.BookRoomAsync(roomId, user);
                if (!updatedBooking) return BadRequest();
                return Ok($"Booked");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error booking room {roomId}");
                return BadRequest(ex);
            }
        }
        /// <summary>
        /// Checks out a user from a room and marks it as to be cleaned.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("check-out/{roomId}")]
        public async Task<IActionResult> CheckOutRoomAsync(int roomId)
        {
            try
            {
                if (roomId == 0) return BadRequest();
                var updatedBooking = await _service.CheckOutRoomAsync(roomId);
                if (!updatedBooking) return BadRequest();
                return Ok($"Checked out");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking out room {roomId}");
                return BadRequest(ex);
            }
        }
    }
}
