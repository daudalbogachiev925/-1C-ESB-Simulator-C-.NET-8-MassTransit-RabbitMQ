using Esb.Core.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Esb.Router.Consumers;

public class PaymentFailedConsumer : IConsumer<PaymentFailed>
{
    private readonly ILogger<PaymentFailedConsumer> _log;

    public PaymentFailedConsumer(ILogger<PaymentFailedConsumer> log)
    {
        _log = log;
    }

    public Task Consume(ConsumeContext<PaymentFailed> context)
    {
        _log.LogError(
            "PaymentFailed: order={OrderId} | reason: {Reason}",
            context.Message.OrderId, context.Message.Reason);
        return Task.CompletedTask;
    }
}
