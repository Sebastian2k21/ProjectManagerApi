using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagerApi.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class ProjectManagerAppTestFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                    typeof(DbContextOptions<DataContext>));
                services.Remove(dbContextDescriptor);
                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbConnection));

                services.Remove(dbConnectionDescriptor);

                services
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<DataContext>((container, options) =>
                {
                    options.UseInMemoryDatabase("UnitTests").UseInternalServiceProvider(container);
                });
            });
            builder.UseEnvironment("Development");
        }
    }
}
