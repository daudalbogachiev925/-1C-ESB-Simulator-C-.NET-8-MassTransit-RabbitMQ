namespace Esb.Core.Envelope;

/// <summary>
/// Обёртка сообщения шины: метаданные + payload.
/// </summary>
public record MessageEnvelope(
    Guid MessageId,
    string Source,
    string Target,
    string Operation,
    string PayloadJson,
    DateTime CreatedAt,
    string Priority = "normal"
);
