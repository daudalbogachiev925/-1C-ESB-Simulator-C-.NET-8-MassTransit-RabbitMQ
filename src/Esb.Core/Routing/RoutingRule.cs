namespace Esb.Core.Routing;

/// <summary>
/// Правило маршрутизации: какие сообщения от источника к приёмнику
/// попадают в какую очередь.
/// </summary>
public record RoutingRule(
    string Source,
    string Target,
    string QueueName,
    bool Enabled = true
);
