using Auction.TimerJob.Context;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Auction.TimerJob.Startup))]

namespace Auction.TimerJob
{
    public class Startup: FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AuctionContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}
