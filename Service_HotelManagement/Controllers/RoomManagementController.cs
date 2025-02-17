using DataHolder.Data.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Service_HotelManagement.Controllers.Shared;
using Service_HotelManagement.Helpers;
using Service_HotelManagement.Interfaces;
using Service_HotelManagement.Providers.Logging;

namespace Service_HotelManagement.Controllers

{   /// <summary>
    /// We traded code readbility for code reusability. This controller is coming from a generic controller. Check the GenericController.cs file for more information.
    /// </summary>
    public class RoomManagementController : GenericController<IRoomManagementService, RoomManagementController, Room>
    {
        public RoomManagementController(IRoomManagementService roomManagementService, ILogger<RoomManagementController> logger) : base(roomManagementService, logger)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? name, [FromQuery] int? size, [FromQuery] bool? available)
        {
            var rooms = await _service.GetAllAsync(name, size, available);
            if (rooms.Count() == 0) _logger.LogWarning("No rooms found");
            return Ok(rooms);
        }

        /* Room is marked automatically as Occupied, to be Cleaned from Booking Service, If we do it manually here - provide details why. Refer to documentation */
        [HttpPut("markAsFree/{roomId}")]
        public async Task<IActionResult> SetAsFreeAsync(int roomId, StringPair? details = null) // If we set room as free after cleaning up we don't need the details
        {
            if (details == null)
                details = new StringPair();
            var roomStatus = PrepareRoomStatusDetailsPair(1, details.Reason, details.Details);
            _logger.LogWarning(null, $"Manually setting room {roomId} as free"); 
            return await ChangeStatusAsync(roomId, roomStatus);
        }

        [HttpPut("markAsOccupied/{roomId}")]
        public async Task<IActionResult> SetAsOccupied(int roomId, [FromBody] StringPair details) // Can be reverted after cleaning or fixing the room that is booked
        {
            if (!string.IsNullOrEmpty(details.Reason))
            {
                var roomStatus = PrepareRoomStatusDetailsPair(2, details.Reason, details.Details);
                _logger.LogWarning(null, $"Manually setting room {roomId} as occupied");
                return await ChangeStatusAsync(roomId, roomStatus);
            }
            return BadRequest("Details are required");
        }

        [HttpPut("markForCleaning/{roomId}")]
        public async Task<IActionResult> SetAsToBeCleanAsync(int roomId, [FromBody] StringPair details) 
        {
            if (!string.IsNullOrEmpty(details.Reason))
            {
                var roomStatus = PrepareRoomStatusDetailsPair(3, details.Reason, details.Details);
                _logger.LogWarning(null, $"Manually setting room {roomId} as to be cleaned");
                return await ChangeStatusAsync(roomId, roomStatus);
            }
            return BadRequest("Details are required");
        }

        [HttpPut("markForMaintenance/{roomId}")]
        public async Task<IActionResult> SetAsToBeFixed(int roomId, [FromBody] StringPair details) 
        {
            if (!string.IsNullOrEmpty(details.Reason))
            {
                var roomStatus = PrepareRoomStatusDetailsPair(4, details.Reason, details.Details);
                _logger.LogWarning(null, $"Manually setting room {roomId} for maintenance");
                return await ChangeStatusAsync(roomId, roomStatus);
            }
            return BadRequest("Details are required");
        }

        [HttpPut("manuallyLock/{roomId}")]
        public async Task<IActionResult> LockTheRoomAsync(int roomId, [FromBody] StringPair details) 
        {
            if (!string.IsNullOrEmpty(details.Reason))
            {
                var roomStatus = PrepareRoomStatusDetailsPair(5, details.Reason, details.Details);
                _logger.LogWarning(null, $"Manually locking room {roomId}");
                return await ChangeStatusAsync(roomId, roomStatus);
            }
            return BadRequest("Details are required");

        }

        /* When a room is manually marked for Cleaning, Maintenance or locking, the status requires additional details. */
        private async Task<IActionResult> ChangeStatusAsync(int roomId, RoomStatusDetailsPair roomStatus)
        {
            try
            {
                if (roomStatus == null) return BadRequest();
                if (roomStatus.RoomStatusDetails == null && !StatusRequiresDetails(roomId)) // If we set room as free, we don't need additional details
                    if (!StatusContainsNameOrDescription(roomStatus))
                        return BadRequest("Additional details are required when manually changing room status");
                var updatedRoom = await _service.ChangeStatusAsync(roomId, roomStatus); 
                if (!updatedRoom) return BadRequest("Room does not exist, is booked in ongoing Booking or data provided is wrong");
                return Ok(updatedRoom);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error changing the status of room {roomId} with status {roomStatus}");
                return BadRequest(ex);
            }

        }

        private bool StatusRequiresDetails(int statusId) // Prolly could be inlined but it's more readable this way
        {
            return statusId != 1; // and also we can easily change the logic if needed
        }

        private bool StatusContainsNameOrDescription(RoomStatusDetailsPair roomStatusDetailsPair)
        {
            return !string.IsNullOrEmpty(roomStatusDetailsPair.RoomStatusDetails.Name) || !string.IsNullOrEmpty(roomStatusDetailsPair.RoomStatusDetails.Description);
        }

        private RoomStatusDetailsPair PrepareRoomStatusDetailsPair(int statusId, string? reason, string? details)
        {
            return new RoomStatusDetailsPair
            {
                RoomStatus = new RoomStatus() { Id = statusId },
                RoomStatusDetails = new RoomStatusDetails()
                {
                    Name = reason ?? string.Empty,
                    Description = details ?? string.Empty
                }
            };
        }
    }

}

