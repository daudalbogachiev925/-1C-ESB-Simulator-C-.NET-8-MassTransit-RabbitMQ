namespace Esb.Core.Messages;

/// <summary>
/// Событие: резервирование не удалось (нет товара).
/// </summary>
public record InventoryRejected(
    Guid OrderId,
    string Reason
);
