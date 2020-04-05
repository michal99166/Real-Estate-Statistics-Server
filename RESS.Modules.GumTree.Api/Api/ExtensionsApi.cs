using Convey.WebApi;
using Convey.WebApi.CQRS;
using Microsoft.AspNetCore.Builder;

namespace RESS.Modules.GumTree.Api.Api
{
    public static class ExtensionsApi
    {
        public static IApplicationBuilder UseGumTreeApi(this IApplicationBuilder app)
        {
            app.UseDispatcherEndpoints(endpoints => endpoints
                .Get<GetWeeklySchedule, ScheduleDto>("schedules/weekly")
                .Post<CreateScheduleSchema>("schedules/schema",
                    afterDispatch: (cmd, ctx) => ctx.Response.Created($"schedules/schema/{cmd.Id}"))
                .Post<GenerateSchedule>("schedules/generate",
                    afterDispatch: (cmd, ctx) => ctx.Response.Created($"schedules/{cmd.Id}")));

            return app;
        }
    }
}
