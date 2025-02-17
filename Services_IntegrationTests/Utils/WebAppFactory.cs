using DataHolder.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Configuration;

namespace Services_IntegrationTests.Utils
{
    public class WebAppFactory<TProgram>
       : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var options =
                services.AddDbContext<DbDataContext>(options =>
                    {
                        options.UseLazyLoadingProxies(); //Instead of running in memory we run on dedicated db which can be more useful in the future
                    });
            }); 

            builder.UseEnvironment("Regression");
        }
    }
}