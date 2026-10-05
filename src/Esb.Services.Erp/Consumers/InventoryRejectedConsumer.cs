using Esb.Core.Messages;
using Esb.Services.Erp.Simulation;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Esb.Services.Erp.Consumers;

/// <summary>
/// ERP слушает InventoryRejected и отменяет заказ.
/// </summary>
public class InventoryRejectedConsumer : IConsumer<InventoryRejected>
{
    private readonly ILogger<InventoryRejectedConsumer> _log;
    private readonly ErpStorage _storage;
    private readonly IPublishEndpoint _bus;

    public InventoryRejectedConsumer(
        ILogger<InventoryRejectedConsumer> log,
        ErpStorage storage,
        IPublishEndpoint bus)
    {
        _log = log;
        _storage = storage;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<InventoryRejected> context)
    {
        var m = context.Message;
        var order = _storage.Get(m.OrderId);

        if (order is null)
        {
            _log.LogWarning("InventoryRejected for unknown order {OrderId}", m.OrderId);
            return;
        }

        _log.LogWarning(
            "ERP: order {OrderId} cancelled — inventory rejected: {Reason}",
            m.OrderId, m.Reason);

        // Публикуем компенсирующее событие
        await context.Publish(new OrderCancelled(
            m.OrderId,
            $"Inventory rejected: {m.Reason}",
            DateTime.UtcNow), context.CancellationToken);
    }
}
