using DataHolder.Data;
using Microsoft.EntityFrameworkCore;
using Service_HotelManagement.Interfaces;
using Service_HotelManagement.Logic;
using Service_HotelManagement.Middleware;
using System.Data.Common;

namespace Service_HotelManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            // Try to open the database connection before starting the application
            try
            {
                CheckDatabaseConnection(host);
                host.Run();
            }
            catch (Exception ex)
            {
                Console.Write("An error occurred while starting the application.");
                Console.Write(ex.ToString());
            }

            finally
            {
                Console.ReadLine();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        /// <summary>
        /// If we cannot access database it's pretty useless to start the application, right?
        /// </summary>
        /// <param name="host"></param>
        private static void CheckDatabaseConnection(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    var context = services.GetRequiredService<DbDataContext>();
                    DbConnection connection = context.Database.GetDbConnection();
                    connection.Open();

                    if (connection.State == System.Data.ConnectionState.Open)
                        logger.LogInformation("Database connection is open.");

                    else
                        logger.LogWarning("Database connection is not open.");

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while opening the database connection.");
                    throw;
                }
                finally
                {
                    var context = services.GetRequiredService<DbDataContext>();
                    context.Database.GetDbConnection().Close();
                }
            }
        }
    }
}
//ANL2025 