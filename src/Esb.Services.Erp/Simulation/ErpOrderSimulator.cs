using Esb.Core.Messages;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Esb.Services.Erp.Simulation;

/// <summary>
/// Симулятор: периодически создаёт новые заказы в ERP и публикует события.
/// Для демо-целей.
/// </summary>
public class ErpOrderSimulator : BackgroundService
{
    private readonly ILogger<ErpOrderSimulator> _log;
    private readonly IPublishEndpoint _bus;
    private readonly ErpStorage _storage;
    private readonly bool _enabled;
    private readonly TimeSpan _interval;
    private readonly Random _rng = new();

    public ErpOrderSimulator(
        ILogger<ErpOrderSimulator> log,
        IPublishEndpoint bus,
        ErpStorage storage,
        IConfiguration config)
    {
        _log = log;
        _bus = bus;
        _storage = storage;
        _enabled = bool.Parse(config["Simulation:Enabled"] ?? "false");
        _interval = TimeSpan.FromSeconds(int.Parse(config["Simulation:IntervalSeconds"] ?? "5"));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_enabled)
        {
            _log.LogInformation("Simulation disabled, skipping order generation");
            return;
        }

        _log.LogInformation("Order simulator running every {Interval}s", _interval.TotalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CreateRandomOrderAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error creating order");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task CreateRandomOrderAsync(CancellationToken ct)
    {
        var order = new OrderCreated(
            OrderId: Guid.NewGuid(),
            CustomerId: Guid.NewGuid(),
            Amount: Math.Round((decimal)(_rng.NextDouble() * 100_000 + 1000), 2),
            CreatedAt: DateTime.UtcNow,
            SourceSystem: "1C_ERP"
        );

        _storage.Add(order);

        _log.LogInformation(
            "Created order {OrderId} | amount={Amount}",
            order.OrderId, order.Amount);

        // Публикуем событие в шину
        await _bus.Publish(order, ct);
    }
}
