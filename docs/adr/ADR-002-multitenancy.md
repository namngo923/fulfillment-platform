# ADR-002 Tenant Isolation Strategy

- Status: Accepted
- Context: The platform serves multiple tenants with shared application code.
- Decision: Propagate tenant context from the API boundary and enforce tenant-scoped storage rules in infrastructure.
- Consequences: Every aggregate root and repository must carry tenant-aware semantics.
