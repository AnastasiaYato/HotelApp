using BasicDataAdder.BasicData;
using DataHolder.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_IntegrationTests.Utils
{
    public static class RegressionDb
    {
        /// <summary>
        /// Here we prepare Regression db so every time we start fresh and with the same data. 
        /// </summary>
        public static void PrepareDb()
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Reg_DbDataContext;Trusted_Connection=True;MultipleActiveResultSets=true";
            var options = new DbContextOptionsBuilder<DbDataContext>();
            options.UseSqlServer(connectionString);
            var context = new DbDataContext(options.Options);
            using (context)
            {
                DbInitializer.Initialize(context);
            }
        }
    }
}
