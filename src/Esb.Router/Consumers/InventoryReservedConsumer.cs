using Esb.Core.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Esb.Router.Consumers;

public class InventoryReservedConsumer : IConsumer<InventoryReserved>
{
    private readonly ILogger<InventoryReservedConsumer> _log;

    public InventoryReservedConsumer(ILogger<InventoryReservedConsumer> log)
    {
        _log = log;
    }

    public Task Consume(ConsumeContext<InventoryReserved> context)
    {
        var m = context.Message;
        _log.LogInformation(
            "InventoryReserved: order={OrderId} | product={ProductId} | qty={Quantity}",
            m.OrderId, m.ProductId, m.Quantity);
        return Task.CompletedTask;
    }
}
