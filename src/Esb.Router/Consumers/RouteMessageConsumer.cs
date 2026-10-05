using Esb.Core.Envelope;
using Esb.Core.Routing;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Esb.Router.Consumers;

/// <summary>
/// Центральный consumer шины.
/// Принимает сообщения с routing_key, определяет очередь и перекладывает туда.
/// </summary>
public class RouteMessageConsumer : IConsumer<MessageEnvelope>
{
    private readonly RoutingTable _routing;
    private readonly ILogger<RouteMessageConsumer> _log;

    public RouteMessageConsumer(
        RoutingTable routing,
        ILogger<RouteMessageConsumer> log)
    {
        _routing = routing;
        _log = log;
    }

    public async Task Consume(ConsumeContext<MessageEnvelope> context)
    {
        var env = context.Message;

        _log.LogInformation(
            "Routing {MessageId} | {Source} → {Target} | op={Operation}",
            env.MessageId, env.Source, env.Target, env.Operation);

        // Находим очередь
        var queue = _routing.ResolveQueue(env.Source, env.Target);

        if (queue is null)
        {
            _log.LogWarning(
                "No route for {Source} → {Target}, message dropped",
                env.Source, env.Target);

            // Публикуем в dead-letter топик
            await context.Publish(new RoutingFailed(
                env.MessageId,
                env.Source,
                env.Target,
                "No route configured"), context.CancellationToken);
            return;
        }

        _log.LogInformation("→ queue: {Queue}", queue);

        // Перекладываем в очередь через send endpoint
        var endpoint = await context.GetSendEndpoint(new Uri($"queue:{queue}"));
        await endpoint.Send(env, context.CancellationToken);

        // Публикуем событие о маршрутизации (для мониторинга)
        await context.Publish(new MessageRouted(
            env.MessageId,
            env.Source,
            env.Target,
            queue,
            DateTime.UtcNow), context.CancellationToken);
    }
}

/// <summary>
/// Событие: маршрутизация не удалась.
/// </summary>
public record RoutingFailed(
    Guid MessageId,
    string Source,
    string Target,
    string Reason
);

/// <summary>
/// Событие: сообщение маршрутизировано.
/// </summary>
public record MessageRouted(
    Guid MessageId,
    string Source,
    string Target,
    string Queue,
    DateTime RoutedAt
);
