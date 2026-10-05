namespace Esb.Core.Messages;

/// <summary>
/// Событие: товар зарезервирован на складе.
/// </summary>
public record InventoryReserved(
    Guid OrderId,
    Guid ProductId,
    int Quantity
);
