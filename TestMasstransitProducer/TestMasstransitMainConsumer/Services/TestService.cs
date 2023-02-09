using MassTransit;
using TestMasstransitEngineConsumer.Consumers;

namespace TestMasstransitEngineConsumer.Services
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
            await CraeteEnginePendingQueue(2);
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("TestService ready.");
                await Task.Delay(10000, stoppingToken);
            }
        }


        private async Task CraeteEnginePendingQueue(int engineId)
        {
            var connector = serviceProvider.GetRequiredService<IReceiveEndpointConnector>();
            Console.WriteLine($"CraeteEnginePendingQueue  set queue:engine_{engineId}");
            var handle = connector.ConnectReceiveEndpoint($"engine_{engineId}", (context, cfg) =>
            {
                cfg.ConfigureConsumer<EnginePendingConsumer>(context);
            });

            await handle.Ready;
        }
    }
}
