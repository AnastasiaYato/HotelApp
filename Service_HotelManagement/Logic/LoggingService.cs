
using DataHolder.Data;
using DataHolder.Data.Health;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Service_HotelManagement.Logic
{
    /// <summary>
    /// My own implementation of ILogger, to log to the database. Pretty cool huh?
    /// </summary>
    public class LoggingService : ILogger
    {
        private readonly DbContextOptionsBuilder<DbDataContext> _options;
        private readonly string _categoryName;

        public LoggingService(DbContextOptionsBuilder<DbDataContext> options, string categoryName)
        {
            _options = options;
            _categoryName = categoryName;
        }
        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;
        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;
            try
            {
                var context = new DbDataContext(_options.Options); // Not ideal but it works, Ig I just really wanted a logging to db
                var logEntry = new LogEntry
                {
                    Timestamp = DateTime.UtcNow,
                    Level = logLevel.ToString(),
                    Message = formatter(state, exception),
                    Exception = exception?.ToString(),
                    Source = _categoryName
                };

                context.LogEntries.Add(logEntry);
                await context.SaveChangesAsync();
                await context.DisposeAsync();
            }
            catch(Exception ex)
            {
                // Well, that's quite ironic - it can happen in case of a db failure or some other issues I didn't think of
            }
        }
    }
}
