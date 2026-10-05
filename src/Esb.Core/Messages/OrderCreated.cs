namespace Esb.Core.Messages;

/// <summary>
/// Событие: создан заказ в 1С:ERP.
/// </summary>
public record OrderCreated(
    Guid OrderId,
    Guid CustomerId,
    decimal Amount,
    DateTime CreatedAt,
    string SourceSystem
);
