# ADR-001 PostgreSQL As Primary Store

- Status: Accepted
- Context: Orders, stock reservations, and tenant-bound reads need transactional consistency.
- Decision: Standardize on PostgreSQL as the main operational datastore.
- Consequences: EF Core mappings and migration discipline become first-class concerns.
