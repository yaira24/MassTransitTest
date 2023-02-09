using MassTransit;
using TestMasstransitEngineConsumer.Models;

namespace TestMasstransitEngineConsumer.Consumers
{
    public class EnginePendingConsumer : IConsumer<EnginePendingModel>
    {
        public EnginePendingConsumer()
        {
        }

        public async Task Consume(ConsumeContext<EnginePendingModel> context)
        {
            try
            {
                Console.WriteLine($"EnginePendingConsumer Consume message, uid: {context.Message.Uid}, build id: {context.Message.BuildId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EnginePendingConsumer Consume get exception: {ex.Message}", ex);
            }
        }

    }
}
