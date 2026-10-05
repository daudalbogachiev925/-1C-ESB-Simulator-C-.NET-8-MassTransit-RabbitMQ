using Esb.Core.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Esb.Router.Consumers;

public class InventoryRejectedConsumer : IConsumer<InventoryRejected>
{
    private readonly ILogger<InventoryRejectedConsumer> _log;

    public InventoryRejectedConsumer(ILogger<InventoryRejectedConsumer> log)
    {
        _log = log;
    }

    public Task Consume(ConsumeContext<InventoryRejected> context)
    {
        _log.LogWarning(
            "InventoryRejected: order={OrderId} | reason: {Reason}",
            context.Message.OrderId, context.Message.Reason);
        return Task.CompletedTask;
    }
}
