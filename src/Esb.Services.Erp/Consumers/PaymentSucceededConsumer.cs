using Esb.Core.Messages;
using Esb.Services.Erp.Simulation;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Esb.Services.Erp.Consumers;

/// <summary>
/// ERP слушает PaymentSucceeded и подтверждает заказ.
/// </summary>
public class PaymentSucceededConsumer : IConsumer<PaymentSucceeded>
{
    private readonly ILogger<PaymentSucceededConsumer> _log;
    private readonly ErpStorage _storage;
    private readonly IPublishEndpoint _bus;

    public PaymentSucceededConsumer(
        ILogger<PaymentSucceededConsumer> log,
        ErpStorage storage,
        IPublishEndpoint bus)
    {
        _log = log;
        _storage = storage;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<PaymentSucceeded> context)
    {
        var m = context.Message;
        var order = _storage.Get(m.OrderId);

        if (order is null)
        {
            _log.LogWarning("PaymentSucceeded for unknown order {OrderId}", m.OrderId);
            return;
        }

        _log.LogInformation(
            "ERP: order {OrderId} CONFIRMED — payment of {Amount} received",
            m.OrderId, m.Amount);

        // Публикуем финальное событие
        await context.Publish(new OrderConfirmed(
            m.OrderId,
            DateTime.UtcNow), context.CancellationToken);
    }
}
