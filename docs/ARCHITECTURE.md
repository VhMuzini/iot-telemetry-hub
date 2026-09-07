# Architecture

## Layers (Clean Architecture)

```
src/
  IoTTelemetryHub.Domain          # Entities, value objects, domain events — no dependencies
  IoTTelemetryHub.Application     # Use cases (CQRS via MediatR), interfaces, validation (FluentValidation)
  IoTTelemetryHub.Infrastructure  # EF Core, Timescale, notification providers, MQTT client
  IoTTelemetryHub.Api             # Controllers, SignalR hubs, auth, composition root
tests/
  IoTTelemetryHub.UnitTests
  IoTTelemetryHub.IntegrationTests
```

Dependency direction: `Api -> Application -> Domain`, with `Infrastructure` implementing interfaces defined in `Application`. Domain has zero external dependencies.

## Core domain concepts

- **Device** — a registered sensor unit (id, type, location, status: online/offline/degraded)
- **SensorReading** — a timestamped value from a device (value, unit, deviceId)
- **AlertRule** — a configurable condition attached to a device or device type (e.g. `value > threshold for N consecutive readings`, `rate of change > X per minute`, `no reading received in N minutes`)
- **AlertEvent** — a fired alert, with severity, the rule that triggered it, and dispatch status

## Alert rule engine

Rather than re-evaluating every rule against every reading (which doesn't scale), rules are indexed by device/device-type at ingestion time, so only the rules relevant to the incoming reading's device are evaluated. Each rule type implements a common `IAlertRule` evaluation interface, keeping the engine open for new rule types without touching the ingestion pipeline (open/closed principle).

## Why TimescaleDB

Sensor readings are a classic time-series workload: high write volume, queries mostly scoped by time range and device. TimescaleDB's hypertables give automatic partitioning and time-bucketed aggregation (`time_bucket`) on top of plain PostgreSQL, without introducing a second database technology to operate.

## Notification dispatch

Notification channels (email, webhook) implement a shared `INotificationChannel` interface, dispatched via a simple strategy pattern based on the alert rule's configured channel. This mirrors how the alerting side of the real system this project is inspired by needed to reach different stakeholders (ops team vs. automated emergency-service webhook) depending on severity.

## Design decisions log

_(to be updated as decisions are made — e.g. why MediatR vs. plain service classes, why device auth uses per-device API keys instead of a shared secret, etc.)_
