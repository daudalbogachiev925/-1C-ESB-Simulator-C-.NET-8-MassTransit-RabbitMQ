using Esb.Core.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Esb.Router.Consumers;

public class PaymentSucceededConsumer : IConsumer<PaymentSucceeded>
{
    private readonly ILogger<PaymentSucceededConsumer> _log;

    public PaymentSucceededConsumer(ILogger<PaymentSucceededConsumer> log)
    {
        _log = log;
    }

    public Task Consume(ConsumeContext<PaymentSucceeded> context)
    {
        var m = context.Message;
        _log.LogInformation(
            "PaymentSucceeded: order={OrderId} | amount={Amount} | paidAt={PaidAt}",
            m.OrderId, m.Amount, m.PaidAt);
        return Task.CompletedTask;
    }
}
