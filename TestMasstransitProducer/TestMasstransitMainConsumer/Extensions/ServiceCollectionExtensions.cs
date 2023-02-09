using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using TestMasstransitEngineConsumer.Consumers;
using TestMasstransitEngineConsumer.Services;

namespace TestMasstransitEngineConsumer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHttpClient();
            services.AddSingleton<TestService>();
            services.AddHostedService<TestService>((sp) => sp.GetRequiredService<TestService>());

            services.AddCors();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });


            services.AddMassTransit(x =>
            {
                x.AddConsumer<EnginePendingConsumer>(typeof(EnginePendingConsumerDefinition));
                x.UsingRabbitMq((context, cfg) =>
                {
                    var rconn = "rabbitmq://admin:admin@localhost:5672";

                    cfg.Host(rconn);
                    cfg.ConfigureEndpoints(context);

                });

            });
        }
    }
}
