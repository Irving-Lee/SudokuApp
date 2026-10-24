# 3. Optimistic Validation (Validate on Check, Not on Move)

**Status:** ✅ Accepted  

## Context

When a user places a number, should we validate Sudoku rules **immediately** (reject invalid moves) or **defer** validation until the user types `check`?

The spec examples explicitly show invalid moves being accepted:
A3 3 Move accepted. ← Accepted despite creating duplicate

check Number 3 already exists in Row A. ← Reported later



## Decision

Use **optimistic validation**:
- Accept all moves meeting basic criteria (valid coordinate, 1-9, not pre-filled)
- Validate Sudoku rules **only** when user types `check` or completes the grid
- Players can create rule-violating states during gameplay

## Consequences

### Pros ✅
- **Spec compliance**: Matches all provided examples exactly
- **Better UX**: Players can experiment and backtrack freely
- **Simpler code**: No validation in `PlaceNumberCommand`
- **Performance**: Validation only when explicitly requested

### Cons ⚠️
- Grid can be in invalid state during play (by design)
- Potential user confusion (some might expect immediate feedback)

### Mitigation
- Clear error messages on `check`
- Automatic validation on win condition
- Encourage `check` usage in prompts

## Alternatives Considered

**Eager validation**: Reject invalid moves immediately. Violates spec examples showing "Move accepted" for duplicates. Rejected for non-compliance.

**Hybrid (warn but allow)**: Adds complexity without clear benefit. Rejected.

## References

- Problem specification (violation examples)
- Fail-fast vs. fail-safe validation strategies