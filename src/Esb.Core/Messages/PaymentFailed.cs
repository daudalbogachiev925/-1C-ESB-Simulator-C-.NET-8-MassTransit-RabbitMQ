namespace Esb.Core.Messages;

/// <summary>
/// Событие: оплата не прошла.
/// </summary>
public record PaymentFailed(
    Guid OrderId,
    string Reason
);
