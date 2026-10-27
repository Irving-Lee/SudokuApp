# 2. Transformation-Based Puzzle Generation

**Status:** ✅ Accepted  

## Context

Must generate valid Sudoku puzzles on demand, within 5 seconds, with sufficient variety. The game supports three difficulty levels with different clue counts.

## Decision

Use **transformation-based generation** instead of backtracking:
1. Generate a base valid solution (mathematical pattern)
2. Apply randomizing transformations:
   - Digit permutation (9! = 362,880 combinations)
   - Row swaps within bands (6³ = 216 combinations)
   - Column swaps within stacks (6³ = 216 combinations)
3. Select clues based on difficulty (40 for Easy, 35 for Medium, 30 for Hard): guarantee ≥1 per row/column, then random fill

**Total variety**: ~16 billion unique solutions

## Consequences

### Pros ✅
- **Fast**: 20-50ms average (100× faster than backtracking)
- **Simple**: ~100 lines of code vs. 300+ for full solver
- **Correct**: Valid by construction (transformations preserve Sudoku rules)
- **Testable**: Easy to verify transformations maintain validity

### Cons ⚠️
- Limited to ~16B solutions (not all 6.67×10²¹ possible Sudoku grids)
- No unique-solution verification (may have multiple solutions)
- Some quality variance despite same clue count

## Alternatives Considered

**Backtracking solver**: More "authentic" but 10× slower (50-500ms) and 3× more complex. Rejected for performance and simplicity.

**Pre-generated puzzle pool**: Instant but requires external data file and limits variety. Rejected for deployment complexity.

## References

- Mathematics of Sudoku (Wikipedia)
- Performance target: < 5 seconds (spec requirement)
