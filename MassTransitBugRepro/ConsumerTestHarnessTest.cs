using MassTransit;
using MassTransit.Testing;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MassTransitBugRepro
{
    public class ConsumerTestHarnessTest
    {
        // this test fails!
        [Fact]
        public async Task CreateConsumerTestHarnessWithoutQueueName_ShouldInvokeConsumerConfigurator()
        {
            var harness = new InMemoryTestHarness();

            var didInvokeConsumerConfigurator = false;

            var consumerHarness = harness.Consumer<SubmitOrderConsumer>((cfg) =>
                {
                    didInvokeConsumerConfigurator = true;
                });

            await harness.Start();
            await harness.Stop();

            Assert.True(didInvokeConsumerConfigurator, "ConsumerConfigurator wasn't called");
        }

        // this tests works (as expected)
        [Fact]
        public async Task CreateConsumerTestHarnessWithQueueName_ShouldInvokeConsumerConfigurator()
        {
            var harness = new InMemoryTestHarness();

            var didInvokeConsumerConfigurator = false;

            var consumerHarness = harness.Consumer<SubmitOrderConsumer>((cfg) =>
                {
                    didInvokeConsumerConfigurator = true;
                },
                "my-queue");

            await harness.Start();
            await harness.Stop();

            Assert.True(didInvokeConsumerConfigurator, "ConsumerConfigurator wasn't called");
        }

        public record SubmitOrder
        {
            public Guid OrderId { get; init; }
        }

        class SubmitOrderConsumer : IConsumer<SubmitOrder>
        {
            public Task Consume(ConsumeContext<SubmitOrder> context)
            {
                return Task.CompletedTask;
            }
        }
    }
}
