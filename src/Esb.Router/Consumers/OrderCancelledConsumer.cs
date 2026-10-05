using Esb.Core.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Esb.Router.Consumers;

public class OrderCancelledConsumer : IConsumer<OrderCancelled>
{
    private readonly ILogger<OrderCancelledConsumer> _log;

    public OrderCancelledConsumer(ILogger<OrderCancelledConsumer> log)
    {
        _log = log;
    }

    public Task Consume(ConsumeContext<OrderCancelled> context)
    {
        _log.LogWarning(
            "OrderCancelled: {OrderId} | reason: {Reason}",
            context.Message.OrderId, context.Message.Reason);
        return Task.CompletedTask;
    }
}
