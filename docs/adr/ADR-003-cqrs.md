# ADR-003 CQRS At Application Boundary

- Status: Proposed
- Context: Order and inventory workflows mix writes with filtered reads.
- Decision: Separate command intent from query intent inside the application layer.
- Consequences: Additional handler/query objects are created, but use-cases remain easier to reason about.
