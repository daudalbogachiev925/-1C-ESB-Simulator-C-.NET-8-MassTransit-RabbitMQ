# 1C ESB Simulator

Симулятор корпоративной шины данных (ESB) для холдинга с несколькими системами 1С.

[![CI](https://github.com/USERNAME/1c-esb-simulator/actions/workflows/ci.yml/badge.svg)](https://github.com/USERNAME/1c-esb-simulator/actions)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

## Проблема

В крупном холдинге работают одновременно 5-10 систем: 1С:ERP, 1С:ЗУП, 1С:УТ, CRM, WMS, SAP, BI. Каждая система — со своей БД, своими справочниками, своим API. Прямая интеграция «каждая с каждой» — это N² связей, которые невозможно поддерживать.

## Решение

Корпоративная шина данных (ESB) — прослойка между системами. Каждая система знает только о шине, шина знает о всех системах. Обмен идёт через события.

Реализует:
- Маршрутизацию сообщений по таблице маршрутов.
- Очереди с гарантированной доставкой.
- Retry с exponential backoff.
- Dead-letter queue для сбойных сообщений.
- Saga-паттерн для распределённых транзакций.
- Мониторинг очередей через RabbitMQ Management UI.

## Архитектура

```
┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│  1С:ERP      │     │  1С:ЗУП      │     │  CRM         │
└──────┬───────┘     └──────┬───────┘     └──────┬───────┘
       │                    │                    │
       └────────────────────┼────────────────────┘
                            │
                            ▼
                 ┌────────────────────┐
                 │   RabbitMQ         │
                 │   (ESB transport)  │
                 └─────────┬──────────┘
                           │
       ┌───────────────────┼───────────────────┐
       │                   │                   │
       ▼                   ▼                   ▼
┌──────────────┐   ┌──────────────┐   ┌──────────────┐
│  Esb.Router  │   │ Esb.Monitor  │   │  Esb.Api     │
└──────────────┘   └──────────────┘   └──────────────┘
```

## Возможности

- **Event-driven** — системы не знают друг о друге, только о шине.
- **Saga-паттерн** — распределённые транзакции с компенсациями.
- **Retry + Dead-letter** — устойчивость к сбоям.
- **Мониторинг** — дашборд очередей в реальном времени.
- **Docker-compose** — поднимается одной командой.
- **Готов к k8s** — Helm-чарты.

## Стек

- .NET 8
- MassTransit 8.x
- RabbitMQ 3.13
- PostgreSQL (для сохранения состояний Saga)
- Serilog (логирование)
- Docker + docker-compose
- xUnit (тесты)

## Быстрый старт

```bash
git clone https://github.com/USERNAME/1c-esb-simulator
cd 1c-esb-simulator
docker-compose up -d
```

RabbitMQ UI: http://localhost:15672 (guest/guest)

## Пример: заказ в 1С:ERP → CRM

```bash
curl -X POST http://localhost:8080/api/messages \
  -H "Content-Type: application/json" \
  -d '{
    "source": "1C_ERP",
    "target": "CRM",
    "operation": "create",
    "payload": {"orderId": "12345", "amount": 50000}
  }'
```

Ответ: `{"messageId": "...", "status": "queued"}`

## Структура

```
1c-esb-simulator/
├── src/
│   ├── Esb.Core/              # общие контракты
│   ├── Esb.Router/            # маршрутизация
│   ├── Esb.Services.Erp/      # эмуляция 1С:ERP
│   ├── Esb.Services.Zup/      # эмуляция 1С:ЗУП
│   ├── Esb.Services.Ut/       # эмуляция 1С:УТ
│   ├── Esb.Monitoring/        # дашборд
│   └── Esb.Api/               # REST API
├── tests/
│   └── Esb.IntegrationTests/
├── k8s/
├── docker-compose.yml
└── docs/
```

## Разработка

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/Esb.Api
```

## Лицензия

MIT — см. [LICENSE](LICENSE).
