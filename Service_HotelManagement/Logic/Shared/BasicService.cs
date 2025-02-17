using DataHolder.Data;

namespace Service_HotelManagement.Logic.Shared
{
    /// <summary>
    /// This class is only to hold the context. Makes it easier to read in derived classes.
    /// </summary>
    public abstract class BasicService
    {
        protected readonly DbDataContext _context;
        public BasicService(DbDataContext context)
        {
            _context = context;
        }
    }
}
