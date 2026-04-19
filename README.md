# fulfillment-platform

Multi-tenant Fulfillment Platform learning project using Clean Architecture, DDD, CQRS, ASP.NET Core, and PostgreSQL-oriented infrastructure boundaries.

## Solution layout

- `src/FulfillmentPlatform.API`: HTTP entry point, middleware, controllers.
- `src/FulfillmentPlatform.Application`: commands, queries, DTOs, shared contracts.
- `src/FulfillmentPlatform.Domain`: entities, aggregates, value objects, rules.
- `src/FulfillmentPlatform.Infrastructure`: persistence, repositories, external adapters.
- `src/Tests`: unit and integration test skeletons.
- `docs`: architecture notes, ADRs, reading notes.

## Current status

This repository currently contains an offline-friendly project skeleton that matches the requested architecture tree. Persistence and tests are intentionally scaffolded so the solution can evolve without pulling extra packages during initial setup.
