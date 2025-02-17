using DataHolder.Data.DbModels;
using Microsoft.AspNetCore.Mvc;
using Service_HotelManagement.Interfaces.Shared;

namespace Service_HotelManagement.Controllers.Shared
{
    /// <summary>
    /// Okay, this is a very, very generic controller. As both services are using the same methods, we can use this controller to avoid code duplication.
    /// DRY principle, right?
    /// </summary>
    /// <typeparam name="TService">Type of the service</typeparam>
    /// <typeparam name="TController">Type of the controller</typeparam>
    /// <typeparam name="TBusinessDbObject">Entity type</typeparam>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GenericController<TService, TController, TBusinessDbObject> : ControllerBase 
        where TService  : IBasicService<TBusinessDbObject> 
        where TController : ControllerBase 
        where TBusinessDbObject  : BasicBusinessDbObject
    {
        protected readonly TService _service;
        protected readonly ILogger<TController> _logger;

        public GenericController(TService service, ILogger<TController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] TBusinessDbObject entity)
        {
            try
            {
                if (entity == null) return BadRequest();
                var addedEntity = await _service.AddAsync(entity);
                return Ok($"New Entity ID: {addedEntity.Id}");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error adding entity - {typeof(TBusinessDbObject).FullName}");
                return BadRequest(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _service.GetByIdAsync(id);
                if (entity == null) return NotFound();
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting entity - {typeof(TBusinessDbObject).FullName}");
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] TBusinessDbObject entity)
        {
            try
            {
                if (entity == null) return BadRequest();
                var updatedEntity = await _service.UpdateAsync(id, entity);
                if (updatedEntity == null) return NotFound();
                return Ok(updatedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating entity - {typeof(TBusinessDbObject).FullName}");
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                return Ok($"Deleted ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting entity - {typeof(TBusinessDbObject).FullName}");
                return BadRequest(ex);
            }
        }
    }
}
