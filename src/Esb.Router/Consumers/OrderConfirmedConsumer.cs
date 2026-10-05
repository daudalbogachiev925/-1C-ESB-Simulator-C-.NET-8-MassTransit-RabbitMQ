using Esb.Core.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Esb.Router.Consumers;

public class OrderConfirmedConsumer : IConsumer<OrderConfirmed>
{
    private readonly ILogger<OrderConfirmedConsumer> _log;

    public OrderConfirmedConsumer(ILogger<OrderConfirmedConsumer> log)
    {
        _log = log;
    }

    public Task Consume(ConsumeContext<OrderConfirmed> context)
    {
        _log.LogInformation(
            "OrderConfirmed: {OrderId} at {ConfirmedAt}",
            context.Message.OrderId, context.Message.ConfirmedAt);
        return Task.CompletedTask;
    }
}
