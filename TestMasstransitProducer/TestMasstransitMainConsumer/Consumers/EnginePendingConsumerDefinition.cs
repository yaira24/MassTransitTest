using MassTransit;

namespace TestMasstransitEngineConsumer.Consumers
{
    public class EnginePendingConsumerDefinition : ConsumerDefinition<EnginePendingConsumer>
    {
        public EnginePendingConsumerDefinition()
        {
        }
    }
}
