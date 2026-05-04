# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Restore dependencies
dotnet restore FulfillmentPlatform.sln

# Build (Release)
dotnet build FulfillmentPlatform.sln --configuration Release --no-restore

# Build (Debug)
dotnet build FulfillmentPlatform.sln

# Run API locally
dotnet run --project src/FulfillmentPlatform.API

# Run all tests
dotnet test FulfillmentPlatform.sln

# Run a single test project
dotnet test src/Tests/FulfillmentPlatform.Tests.Unit
dotnet test src/Tests/FulfillmentPlatform.Tests.Integration

# Start local PostgreSQL (required for integration tests once EF Core is wired up)
docker-compose up -d
```

## Architecture

This is a **multi-tenant fulfillment platform** built with Clean Architecture, DDD, and CQRS on .NET 8/ASP.NET Core. The four layers have strict dependency rules:

```
Domain ← Application ← Infrastructure
                ↑            ↑
               API  ←────────┘
```

### Domain (`src/FulfillmentPlatform.Domain`)
Pure business logic with no framework dependencies. Key aggregates:
- `Order` — aggregate root with a state machine: `Pending → Confirmed → Shipped/Cancelled`. Business rules live in methods like `Order.Confirm()` and `Order.Cancel()`.
- `InventoryItem` — manages stock with `Reserve()` / `AdjustStock()` logic.
- `Shipment` — tracks fulfillment.
- `AuditableEntity` base class — provides `Id`, `TenantId`, `CreatedAt`, and `RowVersion` (for optimistic concurrency) on all aggregates.
- `TenantId` — strong-typed GUID wrapper; every aggregate root carries one.

### Application (`src/FulfillmentPlatform.Application`)
CQRS handlers with no HTTP or persistence dependencies. Commands and Queries are separated under their respective folders. `ICurrentTenant` is the interface for accessing the active tenant in handlers. Mediator dispatch (MediatR) is deferred per ADR-004 — commands/queries are currently wired manually.

### Infrastructure (`src/FulfillmentPlatform.Infrastructure`)
- `AppDbContext` is currently **in-memory** (List<T> stores). EF Core + PostgreSQL migration is planned but not yet implemented.
- `AppDbContextFactory` — factory pattern for context creation.
- Entity configurations under `Persistence/Configurations/` are stubs for future EF Core mapping.
- `MockShippingProvider` implements `IShippingProvider` — the external shipping integration point.

### API (`src/FulfillmentPlatform.API`)
- **Middleware pipeline order matters**: `CorrelationIdMiddleware` (propagates `X-Correlation-Id`) → `TenantResolutionMiddleware` (reads `X-Tenant-Id` header, falls back to config default).
- All DI wiring happens in `ServiceCollectionExtensions`.
- `AppDbContext` is registered as **Singleton** — this is intentional for the in-memory phase but must change to Scoped when EF Core is introduced.

## Multi-Tenancy

Tenant isolation is a first-class concern (ADR-002):
- The active tenant is resolved from the `X-Tenant-Id` HTTP header at the API boundary.
- Default fallback tenant IDs: `...0001` (Production), `...00DE` (Development) — configured in `appsettings.*.json`.
- Every aggregate root has a `TenantId` field. Repositories **must** filter by tenant on all queries.
- `ICurrentTenant.SetTenant()` / `ICurrentTenant.TenantId` — use this interface in handlers, never resolve tenant from headers directly.

## Planned but Not Yet Implemented

Per the ADRs in `docs/adr/`:
- **EF Core + PostgreSQL** — replacing the in-memory `AppDbContext` (ADR-001)
- **MediatR** dispatch — replacing manual command/query wiring (ADR-004)
- **JWT authentication** — tenant claims in bearer tokens (ADR-005)

When adding EF Core, change `AppDbContext` registration from Singleton → Scoped.
