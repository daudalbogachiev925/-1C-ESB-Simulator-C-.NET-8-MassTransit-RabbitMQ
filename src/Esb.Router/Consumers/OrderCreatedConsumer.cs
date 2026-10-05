using Esb.Core.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Esb.Router.Consumers;

/// <summary>
/// Consumer события OrderCreated.
/// Логирует и (при необходимости) запускает Saga.
/// </summary>
public class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    private readonly ILogger<OrderCreatedConsumer> _log;

    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> log)
    {
        _log = log;
    }

    public Task Consume(ConsumeContext<OrderCreated> context)
    {
        var m = context.Message;
        _log.LogInformation(
            "OrderCreated received: {OrderId} | customer={CustomerId} | amount={Amount} | from={Source}",
            m.OrderId, m.CustomerId, m.Amount, m.SourceSystem);

        return Task.CompletedTask;
    }
}
