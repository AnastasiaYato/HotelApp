using DataHolder.Data;
using Microsoft.EntityFrameworkCore;
using Service_HotelManagement.Logic;

namespace Service_HotelManagement.Providers.Logging
{
    /// <summary>
    /// My implementation of the ILoggerProvider.
    /// </summary>
    public class LoggingServiceProvider : ILoggerProvider
    {
        private readonly DbContextOptionsBuilder<DbDataContext> _options = new DbContextOptionsBuilder<DbDataContext>();
        private readonly IConfiguration _configuration;
        public LoggingServiceProvider(IConfiguration configuration)
        {
            _configuration = configuration;
            var options = new DbContextOptionsBuilder<DbDataContext>();
            options.UseSqlServer(_configuration.GetConnectionString("DbDataContext"));
            _options = options;
            
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new LoggingService(_options, categoryName);
        }

        public void Dispose() { }
    }
}
