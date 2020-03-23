using Convey;
using Convey.Logging;
using Convey.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RESS.Shared;
using System.Threading.Tasks;
using RESS.Gumtree;

namespace RESS.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await CreateWebHostBuilder(args)
                .Build()
                .RunAsync();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
            => WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddGumTreeModule()
                    .AddSharedModule()
                    .AddWebApi()
                    .Build())
                .Configure(app => app.UseGumTreeModule()
                    .UseSharedModule()
                    .UseRouting()
                    .UseEndpoints(endpoints => endpoints.MapControllers()))
                .UseLogging(); 
    }
}
