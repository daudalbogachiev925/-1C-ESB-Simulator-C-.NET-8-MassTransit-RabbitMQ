using Esb.Core.Messages;
using Esb.Services.Erp.Simulation;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Esb.Services.Erp.Consumers;

/// <summary>
/// ERP слушает InventoryReserved и обновляет статус заказа.
/// </summary>
public class InventoryReservedConsumer : IConsumer<InventoryReserved>
{
    private readonly ILogger<InventoryReservedConsumer> _log;
    private readonly ErpStorage _storage;

    public InventoryReservedConsumer(
        ILogger<InventoryReservedConsumer> log,
        ErpStorage storage)
    {
        _log = log;
        _storage = storage;
    }

    public Task Consume(ConsumeContext<InventoryReserved> context)
    {
        var m = context.Message;
        var order = _storage.Get(m.OrderId);

        if (order is null)
        {
            _log.LogWarning("InventoryReserved for unknown order {OrderId}", m.OrderId);
            return Task.CompletedTask;
        }

        _log.LogInformation(
            "ERP: inventory reserved for order {OrderId}, waiting for payment...",
            m.OrderId);

        return Task.CompletedTask;
    }
}
