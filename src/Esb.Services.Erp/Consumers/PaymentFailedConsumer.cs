using Esb.Core.Messages;
using Esb.Services.Erp.Simulation;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Esb.Services.Erp.Consumers;

/// <summary>
/// ERP слушает PaymentFailed и отменяет заказ, компенсируя резерв.
/// </summary>
public class PaymentFailedConsumer : IConsumer<PaymentFailed>
{
    private readonly ILogger<PaymentFailedConsumer> _log;
    private readonly ErpStorage _storage;
    private readonly IPublishEndpoint _bus;

    public PaymentFailedConsumer(
        ILogger<PaymentFailedConsumer> log,
        ErpStorage storage,
        IPublishEndpoint bus)
    {
        _log = log;
        _storage = storage;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<PaymentFailed> context)
    {
        var m = context.Message;
        var order = _storage.Get(m.OrderId);

        if (order is null)
        {
            _log.LogWarning("PaymentFailed for unknown order {OrderId}", m.OrderId);
            return;
        }

        _log.LogError(
            "ERP: order {OrderId} CANCELLED — payment failed: {Reason}",
            m.OrderId, m.Reason);

        // Компенсация: отменяем заказ, Inventory вернёт товар
        await context.Publish(new OrderCancelled(
            m.OrderId,
            $"Payment failed: {m.Reason}",
            DateTime.UtcNow), context.CancellationToken);
    }
}
