using Convey;
using Microsoft.AspNetCore.Builder;
using RESS.Modules.GumTree.Api.Api;

namespace RESS.Modules.GumTree.Api
{
    public class Extensions
    {
        public static IConveyBuilder AddSchedulesModule(this IConveyBuilder builder)
            => builder.AddInfrastructure().AddApplication();

        public static IApplicationBuilder UseSchedulesModule(this IApplicationBuilder app)
            => app
                .UseGumTreeApi()
                .UseInfrastructure()
                .UseApplication();
    }
}