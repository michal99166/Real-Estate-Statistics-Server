using Convey;
using Convey.CQRS.Events;
using Microsoft.AspNetCore.Builder;
using RESS.Shared.Exceptions;

namespace RESS.Shared
{
    public static class Extensions
    {
        public static IConveyBuilder AddSharedModule(this IConveyBuilder builder)
        {
            builder
                .AddErrorHandling();

            return builder;
        }

        public static IApplicationBuilder UseSharedModule(this IApplicationBuilder app)
        {
            app.UseErrorHandling();
            return app;
        }
    }
}