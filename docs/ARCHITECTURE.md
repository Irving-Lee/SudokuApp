# Architecture Documentation

## Overview

SudokuConsole is a command-line Sudoku game built with a layered architecture and TDD-focused development.

The application separates:
- Domain logic
- Application services
- User commands
- Console UI
- Game orchestration

```text
┌─────────────────────────────────────────┐
│   Presentation (GameController)         │
├─────────────────────────────────────────┤
│   Infrastructure (ConsoleDisplay)       │
├─────────────────────────────────────────┤
│   Application (Commands, Services)      │
├─────────────────────────────────────────┤
│   Domain (Grid, Cell, Coordinate)       │
└─────────────────────────────────────────┘
```

Dependencies flow inward. Domain models do not depend on UI or infrastructure code.

## Domain Layer

### Coordinate

Coordinate is an immutable value object representing a Sudoku cell position.

Responsibilities:
- Convert user input such as `A1` into zero-based indexes
- Validate row and column bounds
- Provide display-friendly formatting

### Cell

Cell represents one Sudoku grid cell.

Responsibilities:
- Store a value from 1–9 or empty state
- Track whether the cell is pre-filled
- Prevent modification of pre-filled cells

### Grid

Grid is the aggregate root for the 9×9 Sudoku board.

Responsibilities:
- Store all cells
- Read and update cells by coordinate
- Clear editable cells
- Determine whether the grid is complete

## Application Services

### PuzzleGenerator

Generates a complete valid Sudoku solution and derives a playable puzzle from it.

Current difficulty mapping:

| Difficulty | Pre-filled Cells |
|-----------|------------------|
| Easy      | 40               |
| Medium    | 35               |
| Hard      | 30               |

The generator uses transformations of a known valid Sudoku pattern:
- Digit permutation
- Row swaps within bands
- Column swaps within stacks

This approach is fast and produces valid solutions by construction.

### SudokuValidator

Validates the current grid for rule violations.

Validation order:
1. Rows
2. Columns
3. 3×3 subgrids

The validator returns the first violation found, matching the original specification examples.

### HintProvider

Provides one correct value for an empty cell by referencing the generated solution.

The hint provider does not mutate the puzzle.

### LeaderboardService

Stores completed game scores locally in `leaderboard.json`.

Scores include:
- Player name
- Difficulty
- Solve time
- Mistake count
- Completion timestamp

Scores are ranked by:
1. Fastest solve time
2. Fewest mistakes

The leaderboard is local-only and does not sync online.

### Difficulty

`Difficulty` represents the selectable puzzle difficulty (Easy, Medium, Hard) and controls the number of pre-filled cells used by `PuzzleGenerator`.

### GameScore

`GameScore` is an immutable value object capturing a completed game result: player name, difficulty, solve time, mistake count, and completion timestamp.

## Commands

The application uses the Command Pattern for user actions.

```csharp
public interface ICommand
{
    CommandResult Execute();
}
```

Implemented commands:
- `PlaceNumberCommand`
- `ClearCellCommand`
- `HintCommand`
- `CheckCommand`
- `QuitCommand`
- `InvalidCommand`

### CommandFactory

`CommandFactory` converts raw console input into command objects.

Examples:

| Input      | Command              |
|------------|----------------------|
| `A3 4`     | `PlaceNumberCommand` |
| `C5 clear` | `ClearCellCommand`   |
| `hint`     | `HintCommand`        |
| `check`    | `CheckCommand`       |
| `quit`     | `QuitCommand`        |

This keeps input parsing separate from command execution.

## Infrastructure Layer

### ConsoleDisplay

Handles all console input/output.

Responsibilities:
- Display welcome message
- Display Sudoku grid
- Prompt for difficulty
- Read commands
- Display messages
- Display win/game-over messages
- Display leaderboard

Keeping console operations behind `IConsoleDisplay` makes the game controller easier to reason about and keeps UI concerns isolated.

## Presentation Layer

### GameController

Coordinates the main game flow.

Responsibilities:
- Show welcome message
- Ask user to select difficulty
- Generate puzzle and solution
- Start solve timer
- Read and execute commands
- Track mistakes from failed `check` validations
- Detect win condition
- Save completed score to local leaderboard
- Display leaderboard
- Prompt to play again

The controller depends on abstractions:
- `IConsoleDisplay`
- `IPuzzleGenerator`
- `ISudokuValidator`
- `IHintProvider`
- `ILeaderboardService`

## Validation Strategy

The game uses optimistic validation.

This means:
- User moves are accepted if the cell is editable and the value is 1–9.
- Sudoku rule violations are reported when the player runs `check`.

This matches the original problem examples.

Mistakes are counted when:
- The player runs `check`
- The validator finds a rule violation

## Persistence

The application only persists completed leaderboard scores.

Runtime file:

```text
leaderboard.json
```

The app does not persist unfinished game state.

## Design Patterns Used

| Pattern                     | Usage                                             | Reason                                          |
|-----------------------------|---------------------------------------------------|-------------------------------------------------|
| Command Pattern             | User actions                                      | Keeps actions testable and extensible           |
| Factory Pattern             | `CommandFactory`                                  | Centralizes input parsing                       |
| Value Object                | `Coordinate`, `GameScore`                         | Prevents primitive obsession                    |
| Manual Dependency Injection | `Program.cs`                                      | Keeps app lightweight without DI container      |
| Service Abstraction         | Validator, Generator, HintProvider, Leaderboard   | Supports testability and separation of concerns |

## Testing Strategy

The project includes unit tests for:
- Domain models
- Puzzle generation
- Validation
- Hint provider
- Commands
- Command parsing
- Difficulty-based clue counts

Current test count:

```text
156 passing tests
```

## Known Limitations

The following are intentionally out of scope:
- Unique-solution verification
- Advanced human-solving difficulty rating
- Strategic hint explanations
- Multiple violation reporting
- Undo/redo
- Notes/pencil marks
- Save/load unfinished game state
- Online leaderboard/cloud sync
- Color-coded console UI
- Full dependency injection container
- Structured logging infrastructure

## Future Enhancements

Potential future improvements:
- Online leaderboard
- Save/load in-progress games
- Undo/redo
- Notes/pencil marks
- Advanced solving hints
- Human-solving difficulty analysis
- Color-coded display
```

---

### Also fix the trailing PowerShell block (if you keep it as a separate "after updating" note)

Your original ended with an unclosed ``` ``powershell `` fence. Correct version:

````markdown
---

## After updating architecture

Run:

```powershell
dotnet test
```

Then commit with:

```bash
git add docs/ARCHITECTURE.md README.md README.txt .gitignore src tests .gitlab-ci.yml
git commit -m "Update documentation for gameplay enhancements"
```

If you are committing all pending feature code together, use:

```bash
git add .
git restore --staged leaderboard.json 2>$null
git commit -m "Add difficulty, timer, mistakes, and local leaderboard"
```
