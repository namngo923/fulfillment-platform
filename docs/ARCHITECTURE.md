# Architecture

## Goals

- Support multi-tenant fulfillment workflows.
- Keep domain rules isolated from transport and persistence concerns.
- Favor explicit commands/queries over an anemic shared service layer.

## Layers

- API: request routing, middleware, HTTP contracts, tenant/correlation handling.
- Application: use-case contracts, DTOs, orchestration boundaries.
- Domain: aggregate behavior, state transitions, invariants.
- Infrastructure: persistence adapters, repositories, external providers.

## Near-term evolution

1. Replace in-memory `AppDbContext` with EF Core + PostgreSQL.
2. Add handlers/validators for commands and queries.
3. Wire real authentication, tenant isolation, and integration tests.
