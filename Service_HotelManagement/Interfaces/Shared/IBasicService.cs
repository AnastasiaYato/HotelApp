using DataHolder.Data;
using Microsoft.AspNetCore.Mvc;

namespace Service_HotelManagement.Interfaces.Shared
{
    /// <summary>
    /// We hold the basic CRUD operations here.
    /// </summary>
    /// <typeparam name="TBusinessDbObject">Type from our DB</typeparam>
    public interface IBasicService<TBusinessDbObject>
    {
        Task<IEnumerable<TBusinessDbObject>> GetAllAsync();
        Task<TBusinessDbObject> GetByIdAsync(int id);
        Task<TBusinessDbObject> AddAsync(TBusinessDbObject entity);
        Task<TBusinessDbObject> UpdateAsync(int id, TBusinessDbObject entity);
        Task<bool> DeleteAsync(int id);
    }
}
