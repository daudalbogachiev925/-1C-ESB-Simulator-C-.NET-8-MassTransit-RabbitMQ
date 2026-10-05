namespace Esb.Core.Messages;

/// <summary>
/// Событие: заказ отменён с указанием причины.
/// </summary>
public record OrderCancelled(
    Guid OrderId,
    string Reason,
    DateTime CancelledAt
);
