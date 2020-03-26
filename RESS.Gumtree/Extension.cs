using System;
using System.Collections.Generic;
using System.Text;
using Convey;
using Convey.Persistence.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RESS.Gumtree.Infrastructure;
using RESS.Gumtree.Mongo.Documents;
using RESS.Gumtree.Services;
using RESS.Gumtree.Validators;
using RESS.Gumtree.Workers;
using RESS.Gumtree.Workers.Generators;

namespace RESS.Gumtree
{
    public static class Extension
    {
        public static IConveyBuilder AddGumTreeModule(this IConveyBuilder builder)
        {
            builder.Services.AddSingleton<IGumTreeDtoValidator, GumTreeTopicDtoValidator>();
            builder.Services.AddSingleton<IGumTreeService, GumTreeService>();
            builder.Services.AddSingleton<IGumTreeWorkerService, GumTreeWorkerService>();
            builder.Services.AddSingleton<IPagesGenerator, PagesGenerator>();
            builder.Services.AddSingleton<IGumTreeTopicDownloader, GumTreeTopicDownloader>();
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddHostedService<GumTreeHostedService>();

            return builder
                .AddMongo()
                .AddMongoRepository<GumtreeTopicDocument, Guid>("GumTreeTopics")
                .AddInfrastructure();
        }

        public static IApplicationBuilder UseGumTreeModule(this IApplicationBuilder app)
        {
            return app;
        }
        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            var requestsOptions = builder.GetOptions<GumtreeOption>("gumtreeOptions");
            builder.Services.AddSingleton(requestsOptions);
            return builder;
        }
    }
}
