# 1. Command Pattern for User Actions

**Status:** ✅ Accepted  
## Context

The application must handle diverse user actions (place number, clear cell, hint, check, quit) from console input without bloating the controller with complex conditional logic.

## Decision

Implement the **Command Pattern** using an `ICommand` interface and a `CommandFactory` to parse input strings into specific command objects (`PlaceNumberCommand`, `HintCommand`, etc.).

## Consequences

### Pros ✅
- Complete separation of concerns (each command is self-contained)
- Independently testable (100% unit test coverage achieved)
- Extensible: Adding "undo" requires zero changes to existing commands (Open/Closed Principle)
- Uniform error handling via `CommandResult`

### Cons ⚠️
- More classes/files compared to a switch-case statement (~7 command classes vs. 1 method)
- Slight learning curve for developers unfamiliar with the pattern

## Alternatives Considered

**Switch statement in GameController**: Simpler but violates Single Responsibility and is hard to test in isolation. Rejected for maintainability reasons.

## References

- Gang of Four: Command Pattern
- SOLID Principles (Open/Closed, Single Responsibility)
