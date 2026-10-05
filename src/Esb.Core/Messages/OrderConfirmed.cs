namespace Esb.Core.Messages;

/// <summary>
/// Событие: заказ подтверждён.
/// </summary>
public record OrderConfirmed(
    Guid OrderId,
    DateTime ConfirmedAt
);
