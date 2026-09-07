# IoT Telemetry Hub

> Real-time sensor telemetry ingestion, rule-based alerting, and live monitoring API built with .NET 8.

## Problem

Field-deployed sensor networks (river/water level, environmental, industrial) generate continuous readings that need to be ingested reliably, evaluated against safety thresholds, and turned into timely alerts for the people who have to act on them. Most reference projects skip this and just CRUD a database — this one models the actual operational problem: unreliable devices, noisy readings, and alerts that must reach the right channel fast.

This project is a portfolio implementation inspired by a real IoT river-level monitoring system I've worked on, rebuilt as a general-purpose telemetry platform.

## Solution

A .NET 8 Web API that:

- Ingests telemetry from devices over HTTP (and optionally MQTT) with device authentication
- Validates and stores time-series readings
- Evaluates configurable alert rules (threshold, rate-of-change, device-offline) against incoming data
- Pushes real-time updates to connected dashboards via SignalR
- Dispatches alerts through pluggable notification channels (email/webhook)
- Exposes historical data and device status via a documented REST API

## Tech stack

| Layer | Choice |
|---|---|
| API | ASP.NET Core 8 Web API |
| Architecture | Clean Architecture (Domain / Application / Infrastructure / API) |
| Data | PostgreSQL + TimescaleDB extension for time-series data |
| ORM | Entity Framework Core + Npgsql |
| Real-time | SignalR |
| Messaging (optional) | MQTT via MQTTnet, for simulating real device ingestion |
| Auth | JWT bearer (device tokens + user auth for dashboard) |
| Testing | xUnit, FluentAssertions, Testcontainers (Postgres) for integration tests |
| Docs | Swagger / OpenAPI |
| Infra | Docker Compose (API + Postgres/Timescale) |
| CI | GitHub Actions (build, test, lint) |

## Architecture

See [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md) for the full breakdown of layers, the alerting rule engine design, and key design decisions.

High level flow:

```
Device --> POST /api/telemetry --> Ingestion Pipeline --> TimescaleDB
                                          |
                                          v
                                  Alert Rule Engine
                                    /            \
                          SignalR Hub        Notification Dispatcher
                          (live dashboard)    (email / webhook)
```

## Status

🚧 In active development. Core domain, ingestion flow, alert engine, SignalR broadcast, Docker and CI are scaffolded. See [Roadmap](#roadmap) and the repo's Issues tab for what's left.

## Roadmap

- [x] Domain model: Device, SensorReading, AlertRule, AlertEvent
- [ ] EF Core + Timescale hypertable setup — DbContext/configurations in place, hypertable migration still to be generated
- [x] Telemetry ingestion endpoint + validation pipeline
- [x] Alert rule engine (threshold, rate-of-change, offline-device rules)
- [x] SignalR live dashboard hub
- [x] Notification dispatcher (email + webhook channels)
- [ ] Device auth (API keys/JWT)
- [ ] Integration tests with Testcontainers — project scaffolded, first test still to be written
- [x] Docker Compose for local run
- [x] GitHub Actions CI pipeline
- [ ] Seed/demo data + simulated device script

## What I learned

_(filled in as the project progresses — the goal is to document real trade-offs made, e.g. why Timescale over plain Postgres partitioning, how the alert engine avoids re-evaluating the full rule set per reading, etc.)_

## Running locally

```bash
docker compose up -d
dotnet run --project src/IoTTelemetryHub.Api
```

Swagger UI available at `/swagger` once running.

## License

MIT
