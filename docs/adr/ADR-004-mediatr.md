# ADR-004 Mediator-Style Dispatch

- Status: Proposed
- Context: The current skeleton exposes commands and queries but not dispatch plumbing.
- Decision: Introduce mediator-style request dispatch only after the core workflows and validations are stable.
- Consequences: Initial code stays explicit; later refactors can centralize pipeline behaviors.
