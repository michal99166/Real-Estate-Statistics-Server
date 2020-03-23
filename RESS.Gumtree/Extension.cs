using System;
using System.Collections.Generic;
using System.Text;
using Convey;
using Convey.Persistence.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RESS.Gumtree.Mongo.Documents;
using RESS.Gumtree.Services;
using RESS.Gumtree.Validators;

namespace RESS.Gumtree
{
    public static class Extension
    {
        public static IConveyBuilder AddGumTreeModule(this IConveyBuilder builder)
        {
            builder.Services.AddSingleton<IGumTreeDtoValidator, GumTreeTopicDtoValidator>();
            builder.Services.AddSingleton<IGumTreeService, GumTreeService>();
            builder.Services.AddControllers().AddNewtonsoftJson();

            return builder
                .AddMongo()
                .AddMongoRepository<GumtreeTopicDocument, Guid>("GumTreeTopics");
        }

        public static IApplicationBuilder UseGumTreeModule(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
