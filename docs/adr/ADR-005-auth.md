# ADR-005 JWT-Based API Authentication

- Status: Proposed
- Context: Partner-facing APIs need stateless auth that can carry tenant claims.
- Decision: Use JWT bearer tokens at the API edge, with tenant identity resolved from trusted claims or headers in lower environments.
- Consequences: Auth bootstrap remains simple while preserving a path to real IdP integration.
