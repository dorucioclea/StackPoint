using Data.Persistence.Classes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Data.Persistence
{
    class Program
    {

        public static IConfiguration Configuration { get; set; }

        static async Task Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions()
                        .Configure<DataBase>(Configuration.GetSection("DataBase"));
                    services.AddHostedService<SubscribeSevice>();
                    services.AddEntityFrameworkNpgsql()
                        .AddDbContext<AddContext>()
                        .BuildServiceProvider();
                });

            await builder
                .RunConsoleAsync();
        }
    }
}
