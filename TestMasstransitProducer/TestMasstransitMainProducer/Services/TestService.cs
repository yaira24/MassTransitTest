using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;
using TestMasstransitMainProducer.Models;

namespace TestMasstransitMainProducer.Services
{
    public class TestService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        public TestService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int index1 = 0;
            int index2 = 0;

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("TestService ready.");
                EnginePendingModel pendingModel1 = new EnginePendingModel() { Uid = "test1", Bs64Features = "", BuildId = 1, Chanel = "1", EnrollId = index1++ };
                await PublicEnginePending(1, pendingModel1); ;
                await Task.Delay(10000, stoppingToken);
                EnginePendingModel pendingModel2 = new EnginePendingModel() { Uid = "test2", Bs64Features = "", BuildId = 2, Chanel = "2", EnrollId = index2++};
                await PublicEnginePending(2, pendingModel2);
            }
        }


        private async Task PublicEnginePending(int engineId, EnginePendingModel pendingModel)
        {
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var busControl = serviceScope.ServiceProvider.GetRequiredService<IBusControl>();

                await busControl.StartAsync(source.Token);
                var endpoint = await busControl.GetSendEndpoint(new Uri($"queue:engine_{engineId}"));
                await endpoint.Send(pendingModel, source.Token);
            }
        }
    }
}
