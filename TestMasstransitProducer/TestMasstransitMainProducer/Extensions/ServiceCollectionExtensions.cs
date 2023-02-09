using MassTransit.JobService;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using TestMasstransitMainProducer.Services;

namespace TestMasstransitMainProducer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                //options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.MaxDepth = 1024;
            });


            services.AddHttpClient();


            //services.AddTransient<HubEnginesWorker>();
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
