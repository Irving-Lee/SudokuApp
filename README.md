# Sudoku Console Game

> A C# command-line Sudoku game with difficulty-based puzzle generation, validation commands, hints, solve-time tracking, and a local leaderboard.

---

## 🚀 Quick Start

### Prerequisites

- **.NET 10.0 SDK or later**
- Any OS: Windows, Linux, macOS

### Build & Run

```bash
# Navigate to project root
cd SudokuConsole

# Build the solution
dotnet build

# Run the game
dotnet run --project src/SudokuConsole

# Run all tests
dotnet test

# Run tests and collect coverage (optional)
dotnet test --collect:"XPlat Code Coverage"
```

### Example Output (excerpt)

The game displays all nine rows; this excerpt shows only the first three.

```text
Welcome to Sudoku!

Select difficulty:
1. Easy (40 clues)
2. Medium (35 clues)
3. Hard (30 clues)

Enter choice (1-3): 2

Here is your puzzle:
    1 2 3 4 5 6 7 8 9
  A _ 3 _ _ 7 _ _ _ _
  B 6 _ _ 1 9 5 _ _ _
  C _ 9 8 _ _ _ _ 6 _
  ...

Enter command (e.g., A3 4, C5 clear, hint, check, quit):

```

---

## 🎮 How to Play

### Difficulty Selection

At the start of each game, choose a difficulty:

| Difficulty | Pre-filled Cells | Description |
|-----------|------------------|-------------|
| Easy | 40 | More clues, easier to solve |
| Medium | 35 | Balanced puzzle |
| Hard | 30 | Fewer clues, more challenging |

The difficulty controls the number of pre-filled cells. It does not currently use a human-solving complexity model.

### Commands

| Command | Description | Example |
|---------|-------------|---------|
| `[Cell] [Number]` | Place a number (1-9) in a cell | `A3 4` |
| `[Cell] clear` | Clear a cell | `C5 clear` |
| `hint` | Request a hint | `hint` |
| `check` | Validate current grid | `check` |
| `quit` | Exit the game | `quit` |
| Timer | Tracks elapsed time | — |
| Mistake Counter | Counts failed `check` validations and ends the game after 5 mistakes | — |
| Local Leaderboard | Saves completed game scores locally by difficulty | — |
| Hints | Reveals a correct value for an empty cell without modifying the grid | — |

### Coordinate System
- **Rows**: A–I (top to bottom)
- **Columns**: 1–9 (left to right)
- **Example**: `E5` = Row E, Column 5 (center cell)

### Game Rules
- ✅ Each row must contain numbers 1–9 without duplicates
- ✅ Each column must contain numbers 1–9 without duplicates
- ✅ Each 3×3 subgrid must contain numbers 1–9 without duplicates
- ✅ Pre-filled cells cannot be modified
- ✅ Game ends when the grid is completely and correctly filled

---

## 🏆 Leaderboard

When a puzzle is completed successfully, the game asks for a player name and saves the result locally.

Leaderboard ranking is based on:

1. Fastest solve time
2. Fewest mistakes

Scores are grouped by difficulty.

A local runtime file is created:

```text
leaderboard.json
```

The file is stored in the application's current working directory.

## 🏗️ Architecture Overview

### Design Philosophy

The project uses a layered architecture with clear separation of concerns.

```text
┌─────────────────────────────────────────┐
│   Presentation (GameController)         │  ← Orchestrates game loop
├─────────────────────────────────────────┤
│   Infrastructure (ConsoleDisplay)       │  ← UI rendering
├─────────────────────────────────────────┤
│   Application (Commands, Services)      │  ← Use cases & business logic
├─────────────────────────────────────────┤
│   Domain (Grid, Cell, Coordinate)       │  ← Core business entities
└─────────────────────────────────────────┘
```

### Key Patterns Used

| Pattern                     | Where                  | Why                                       |
| --------------------------- | ---------------------- | ----------------------------------------- |
| Command Pattern             | User actions           | Encapsulation, testability, extensibility |
| Factory Pattern             | CommandFactory         | Centralized input parsing                 |
| Value Object                | Coordinate             | Type safety and immutability              |
| Manual Dependency Injection | Program/GameController | Simplicity without container overhead     |

---


## 📊 Project Structure


````markdown
SudokuConsole/
├── LICENSE
├── README.md
├── SudokuConsole.slnx
├── docs/
│   ├── ARCHITECTURE.md
│   └── adr/
│       ├── 0001-command-pattern-for-user-actions.md
│       ├── 0002-backtracking-puzzle-generation.md
│       └── 0003-optimistic-validation-strategy.md
├── src/
│   └── SudokuConsole/
│       ├── Program.cs
│       ├── Application/
│       │   ├── Commands/
│       │   │   ├── CheckCommand.cs
│       │   │   ├── ClearCellCommand.cs
│       │   │   ├── CommandFactory.cs
│       │   │   ├── CommandResult.cs
│       │   │   ├── HintCommand.cs
│       │   │   ├── ICommand.cs
│       │   │   ├── InvalidCommand.cs
│       │   │   ├── PlaceNumberCommand.cs
│       │   │   └── QuitCommand.cs
│       │   └── Services/
│       │       ├── Difficulty.cs
│       │       ├── GameScore.cs
│       │       ├── HintProvider.cs
│       │       ├── HintResult.cs
│       │       ├── IHintProvider.cs
│       │       ├── ILeaderboardService.cs
│       │       ├── IPuzzleGenerator.cs
│       │       ├── ISudokuValidator.cs
│       │       ├── LeaderboardService.cs
│       │       ├── PuzzleGenerator.cs
│       │       ├── SudokuValidator.cs
│       │       └── ValidationResult.cs
│       ├── Domain/
│       │   └── Models/
│       │       ├── Cell.cs
│       │       ├── Coordinate.cs
│       │       └── Grid.cs
│       ├── Infrastructure/
│       │   └── UI/
│       │       ├── ConsoleDisplay.cs
│       │       └── IConsoleDisplay.cs
│       └── Presentation/
│           └── GameController.cs
├── tests/
│   └── SudokuConsole.Tests/
│       ├── Application/
│       │   ├── Commands/
│       │   │   ├── CheckCommandTests.cs
│       │   │   ├── ClearCellCommandTests.cs
│       │   │   ├── CommandFactoryTests.cs
│       │   │   ├── HintCommandTests.cs
│       │   │   ├── PlaceNumberCommandTests.cs
│       │   │   └── QuitCommandTests.cs
│       │   │
│       │   └── Services/
│       │       ├── HintProviderTests.cs
│       │       ├── PuzzleGeneratorTests.cs
│       │       └── SudokuValidatorTests.cs
│       │
│       └── Domain/
│           ├── CellTests.cs
│           ├── CoordinateTests.cs
│           └── GridTests.cs
````

---

## 🧪 Testing

### Test Coverage Summary
```text
Tested areas:
- Domain models: Coordinate, Cell, Grid
- Application services: Puzzle generation, validation, hints
- Commands: Place, clear, hint, check, quit, invalid input, command parsing
- Difficulty generation: Easy/Medium/Hard clue counts
```

The current test project does not include dedicated tests for `GameController` or file-based leaderboard persistence.

The suite also uses NUnit parameterized tests for selected inputs and boundary cases.

### Running Tests
```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific test class
dotnet test --filter "FullyQualifiedName~CoordinateTests"

# Run and collect coverage (requires the test project's Coverlet collector)
dotnet test --collect:"XPlat Code Coverage"
```

---

## 🎯 Design Decisions & Assumptions

### ✅ **Decisions Made**

#### 1. **Optimistic Validation**
- **Decision**: Moves are accepted without immediate validation; violations only reported on `check` command
- **Rationale**: Per spec examples, invalid moves are allowed (e.g., placing duplicate 3 in Row A)
- **Trade-off**: Users can create invalid states, but aligns with UX requirement

#### 2. **Puzzle Generation Algorithm**
- **Algorithm**: Randomized valid solution via digit permutation and row/column swaps within bands/stacks
- **Performance**: Uses transformations of a valid solution rather than a backtracking search; no benchmark is included.
- **Trade-off**: Didn't implement full backtracking solver or unique-solution verification (see assumptions below)

#### 3. **Clue Selection Strategy**
- **Decision**: Guarantee at least one clue per row and column, then randomly fill to the count for the selected difficulty (40 Easy, 35 Medium, 30 Hard)
- **Rationale**: Improves playability without over-constraining
- **Not in spec**: This is a quality enhancement

#### 4. **Immutable Coordinate Value Object**
- **Implementation**: `readonly record struct`
- **Why**: Prevents accidental mutation, enables value equality, modern C# idiom
- **Benefit**: Type-safe coordinate handling (no swapping row/column bugs)

#### 5. **Command Pattern over Switch Statement**
- **Why**: Extensible (new commands = new class), independently testable, adheres to SRP and OCP
- **Trade-off**: More files, but cleaner separation and easier to maintain

#### 6. **Manual Dependency Injection**
- **Why**: Console app doesn't need full DI container overhead
- **Approach**: Constructor injection with interfaces for testability
- **Benefit**: Demonstrates SOLID principles without external dependencies


#### 7. **Difficulty by Clue Count**
- **Decision**: Difficulty is determined by the number of pre-filled cells.
- **Mapping**:
  - Easy: 40 clues
  - Medium: 35 clues
  - Hard: 30 clues
- **Rationale**: This keeps generation fast and simple while providing a meaningful difference between difficulties. The clue count is fixed for each difficulty, but generated puzzles are randomized.
- **Trade-off**: Difficulty is not calculated using advanced human-solving techniques.

#### 8. **Timer and Leaderboard**
- **Decision**: Track solve time and save completed scores to a local JSON leaderboard.
- **Rationale**: Adds replay value and lightweight progression without requiring a database or online service.
- **Trade-off**: Leaderboard data is local to the machine and not shared between users.

#### 9. **Mistake Counter**
- **Decision**: Mistakes are counted when the player runs `check` and the validator finds a rule violation.
- **Rationale**: This preserves the original optimistic validation design where moves are accepted first and rule validation is explicitly requested.
- **Trade-off**: Only failed `check` commands that report rule violations increment the counter. Incorrect placements and a failed completion validation do not independently add mistakes.

---

## 📝 Assumptions and Out of Scope

The following features were deliberately excluded because they were not explicitly required by the original specification or would add complexity beyond the current scope.

### Not Implemented

#### ❌ Unique Solution Verification

- The original specification does not require unique-solution validation.
- The generator provides a valid solution for each puzzle but does not prove it is the only possible solution.
- Adding unique-solution verification would require a solver that counts solutions and would increase generation complexity.

#### ❌ Multiple Violation Reporting

- The specification examples show a single violation message.
- Validation reports the first rule violation found.
- Violation priority is: Row → Column → Subgrid.

#### ❌ Advanced Hint Algorithms

- The hint feature reveals one correct value for an empty cell.
- It does not explain solving techniques.
- Strategic hints such as naked singles, hidden singles, pairs, or X-wing are out of scope.

#### ❌ Advanced Difficulty Rating

- Difficulty is currently based on the number of pre-filled cells:
  - Easy: 40 clues
  - Medium: 35 clues
  - Hard: 30 clues
- The app does not analyze human-solving complexity.
- Techniques such as naked pairs, hidden singles, X-wing, or solving-depth scoring are not used.

#### ❌ Online Leaderboard / Cloud Sync

- Scores are stored locally in `leaderboard.json`.
- There is no server-side leaderboard, database, account system, or cloud sync.

#### ❌ Save/Load In-Progress Game State

- The app does not currently save or resume unfinished puzzles.
- Only completed scores are persisted to the local leaderboard.
- Full game-state persistence is out of scope.

#### ❌ Undo/Redo Functionality

- Undo/redo is not part of the original specification.
- The Command Pattern makes this a natural future enhancement.

#### ❌ Notes / Pencil Marks

- Candidate notes are not currently supported.
- This would require extending the `Cell` model, command parsing, and grid display format.

#### ❌ Color-Coded Console Output

- Console output is plain text.
- Color-coded display for givens, user entries, hints, and errors is a possible future enhancement.

#### ❌ Full Dependency Injection Container

- The app uses manual constructor injection.
- A full DI container such as `Microsoft.Extensions.DependencyInjection` was avoided to keep the console app lightweight.

#### ❌ Extensive Logging Infrastructure

- The app does not include file-based or structured logging.
- User-facing command results and validation messages are sufficient for the current scope.
---

### 🔧 **Technical Assumptions**

- **Platform**: Cross-platform (.NET 10.0+, runs on Windows/Linux/macOS)
- **Input Handling**: Case-insensitive, whitespace-tolerant (UX improvement)
- **Error Messages**: Follow the formats shown in the specification examples
- **Grid Display**: Uses row labels A–I and column labels 1–9.
- **Test Framework**: NUnit (industry standard, rich assertion library)

---

## 🚀 Future Enhancements (Post-MVP)

If this project were to evolve, potential additions include:

- [ ] Online leaderboard or cloud sync
- [ ] Advanced difficulty analysis based on solving techniques
- [ ] Undo/Redo functionality
- [ ] Save/Load game state
- [ ] Notes/Pencil marks
- [ ] Color-coded terminal output
- [ ] Strategic hint explanations

---

## 📚 Documentation

- **[ARCHITECTURE.md](docs/ARCHITECTURE.md)** - Detailed technical design
- **[ADR 0001: Command Pattern](docs/adr/0001-command-pattern-for-user-actions.md)**
- **[ADR 0002: Puzzle Generation](docs/adr/0002-backtracking-puzzle-generation.md)**
- **[ADR 0003: Validation Strategy](docs/adr/0003-optimistic-validation-strategy.md)**

---

## 🛠️ Development

### Implementation Practices
- Uses common C# naming conventions.
- Keeps command handling, domain models, services, and console presentation in separate areas.
- Uses interfaces for key services and the console display to support substitution and testing.
- Includes XML documentation on public APIs where it clarifies usage.

## Recent Enhancements

- Added difficulty selection: Easy, Medium, Hard
- Added solve timer
- Added mistake counter with game-over threshold
- Added local leaderboard saved to `leaderboard.json`

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

## 👤 Author

Developed as a demonstration of C# application structure, command-based user actions, domain modeling, and automated testing.

**Assessment Submission**: Irving Lee - 27/10/26
