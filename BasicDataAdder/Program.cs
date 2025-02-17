using BasicDataAdder.BasicData;
using DataHolder.Data;
using DataHolder.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BasicDataAdder
{
    internal class Program
    {
        /// <summary>
        /// Run this to first create database and add basic data. You can also use it as Hard Reset.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Prod_DbDataContext;Trusted_Connection=True;MultipleActiveResultSets=true";
            var options = new DbContextOptionsBuilder<DbDataContext>();
            options.UseSqlServer(connectionString);
            var context = new DbDataContext(options.Options);
            using (context)
            {
                DbInitializer.Initialize(context);
            }
            Console.ReadLine();
        }
      
    }
}
//ANL2025 