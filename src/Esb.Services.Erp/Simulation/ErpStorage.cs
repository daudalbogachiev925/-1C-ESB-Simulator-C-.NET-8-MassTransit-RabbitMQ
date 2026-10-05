using System.Collections.Concurrent;
using Esb.Core.Messages;

namespace Esb.Services.Erp.Simulation;

/// <summary>
/// In-memory хранилище заказов 1С:ERP.
/// В реальной системе — это база 1С.
/// </summary>
public class ErpStorage
{
    private readonly ConcurrentDictionary<Guid, OrderCreated> _orders = new();

    public void Add(OrderCreated order) => _orders[order.OrderId] = order;

    public OrderCreated? Get(Guid orderId) =>
        _orders.TryGetValue(orderId, out var order) ? order : null;

    public IReadOnlyCollection<OrderCreated> All => _orders.Values.ToList();

    public int Count => _orders.Count;
}
