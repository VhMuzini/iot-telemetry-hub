# Integration tests

Spins up a real PostgreSQL instance via Testcontainers and exercises the API
through `WebApplicationFactory<Program>`. Requires Docker to be running
locally / available on the CI runner.

Planned first test: `POST /api/telemetry` for a registered device persists a
reading and returns 202, then a threshold rule attached to that device type
produces a queryable `AlertEvent`.
