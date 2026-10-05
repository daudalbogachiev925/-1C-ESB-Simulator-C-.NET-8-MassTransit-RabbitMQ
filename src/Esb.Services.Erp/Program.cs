using Esb.Services.Erp.Consumers;
using Esb.Services.Erp.Simulation;
using MassTransit;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] [ERP] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

// In-memory хранилище заказов
builder.Services.AddSingleton<ErpStorage>();

builder.Services.AddMassTransit(x =>
{
    // Регистрируем consumers — реакции ERP на события других систем
    x.AddConsumer<InventoryReservedConsumer>();
    x.AddConsumer<InventoryRejectedConsumer>();
    x.AddConsumer<PaymentSucceededConsumer>();
    x.AddConsumer<PaymentFailedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
        cfg.Host(host, "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:User"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });

        cfg.UseMessageRetry(r => r.Exponential(
            retryLimit: 5,
            minInterval: TimeSpan.FromSeconds(1),
            maxInterval: TimeSpan.FromSeconds(30),
            intervalDelta: TimeSpan.FromSeconds(2)));

        cfg.ConfigureEndpoints(context);
    });
});

// Симулятор: раз в 5 секунд создаёт новый заказ (для демо)
builder.Services.AddHostedService<ErpOrderSimulator>();

var host = builder.Build();

Log.Information("ESB ERP Service started");
await host.RunAsync();
