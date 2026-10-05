namespace Esb.Core.Messages;

/// <summary>
/// Событие: оплата успешно проведена.
/// </summary>
public record PaymentSucceeded(
    Guid OrderId,
    decimal Amount,
    DateTime PaidAt
);
