using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Services;

/// <summary>
/// Generates a Sudoku puzzle and its complete solution.
/// </summary>
public sealed class PuzzleGenerator : IPuzzleGenerator
{
    private const int GridSize = 9;
    private const int PrefilledCellCount = 30;

    /// <inheritdoc />
    private static Grid CreatePuzzle(Grid solution, Difficulty difficulty)
    {
        var puzzle = new Grid();
        var random = new Random();
        var selected = new HashSet<(int Row, int Column)>();
        var prefilledCellCount = GetPrefilledCellCount(difficulty);

        // Step 1: Guarantee one clue per row
        for (var row = 0; row < GridSize; row++)
        {
            var column = random.Next(GridSize);
            selected.Add((row, column));
        }

        // Step 2: Guarantee one clue per column (if not already covered)
        for (var column = 0; column < GridSize; column++)
        {
            var hasClue = false;

            for (var row = 0; row < GridSize; row++)
            {
                if (selected.Contains((row, column)))
                {
                    hasClue = true;
                    break;
                }
            }

            if (!hasClue)
            {
                var availableRows = new List<int>();

                for (var row = 0; row < GridSize; row++)
                {
                    if (!selected.Contains((row, column)))
                    {
                        availableRows.Add(row);
                    }
                }

                if (availableRows.Count > 0)
                {
                    var randomRow = availableRows[random.Next(availableRows.Count)];
                    selected.Add((randomRow, column));
                }
            }
        }

        // Step 3: Fill remaining slots randomly until target clue count is reached
        if (selected.Count < prefilledCellCount)
        {
            var allCoordinates = new List<(int Row, int Column)>();

            for (var row = 0; row < GridSize; row++)
            {
                for (var column = 0; column < GridSize; column++)
                {
                    if (!selected.Contains((row, column)))
                    {
                        allCoordinates.Add((row, column));
                    }
                }
            }

            Shuffle(allCoordinates, random);

            var needed = prefilledCellCount - selected.Count;
            for (var index = 0; index < needed && index < allCoordinates.Count; index++)
            {
                selected.Add(allCoordinates[index]);
            }
        }

        // Step 4: Place the selected clues
        foreach (var (row, column) in selected)
        {
            var coordinate = new Coordinate(row, column);
            var value = solution.GetCell(coordinate).Value!.Value;
            puzzle.SetPrefilledCell(coordinate, value);
        }

        return puzzle;
    }


    private static Grid CreateCompleteSolution()
    {
        var random = new Random();
        var solution = new Grid();

        // Start with the base valid pattern
        for (var row = 0; row < GridSize; row++)
        {
            for (var column = 0; column < GridSize; column++)
            {
                var value = (row * 3 + row / 3 + column) % GridSize + 1;
                solution.SetCell(new Coordinate(row, column), value);
            }
        }

        // Apply randomizing transformations that preserve Sudoku validity

        // 1. Randomly permute digits (e.g., swap all 1s with 5s, all 5s with 1s)
        var digitMapping = Enumerable.Range(1, 9).ToList();
        Shuffle(digitMapping, random);

        var remappedSolution = new Grid();
        for (var row = 0; row < GridSize; row++)
        {
            for (var column = 0; column < GridSize; column++)
            {
                var coord = new Coordinate(row, column);
                var originalValue = solution.GetCell(coord).Value!.Value;
                var newValue = digitMapping[originalValue - 1];
                remappedSolution.SetCell(coord, newValue);
            }
        }

        solution = remappedSolution;

        // 2. Randomly swap rows within each band (rows 0-2, 3-5, 6-8)
        for (var band = 0; band < 3; band++)
        {
            var rowsInBand = new[] { band * 3, band * 3 + 1, band * 3 + 2 };
            Shuffle(rowsInBand, random);

            var swappedSolution = new Grid();
            for (var row = 0; row < GridSize; row++)
            {
                var sourceRow = row;
                if (row >= band * 3 && row < band * 3 + 3)
                {
                    sourceRow = rowsInBand[row - band * 3];
                }

                for (var column = 0; column < GridSize; column++)
                {
                    var value = solution.GetCell(new Coordinate(sourceRow, column)).Value!.Value;
                    swappedSolution.SetCell(new Coordinate(row, column), value);
                }
            }

            solution = swappedSolution;
        }

        // 3. Randomly swap columns within each stack (columns 0-2, 3-5, 6-8)
        for (var stack = 0; stack < 3; stack++)
        {
            var columnsInStack = new[] { stack * 3, stack * 3 + 1, stack * 3 + 2 };
            Shuffle(columnsInStack, random);

            var swappedSolution = new Grid();
            for (var row = 0; row < GridSize; row++)
            {
                for (var column = 0; column < GridSize; column++)
                {
                    var sourceColumn = column;
                    if (column >= stack * 3 && column < stack * 3 + 3)
                    {
                        sourceColumn = columnsInStack[column - stack * 3];
                    }

                    var value = solution.GetCell(new Coordinate(row, sourceColumn)).Value!.Value;
                    swappedSolution.SetCell(new Coordinate(row, column), value);
                }
            }

            solution = swappedSolution;
        }

        return solution;
    }


    private static Grid CreatePuzzle(Grid solution)
    {
        var puzzle = new Grid();
        var random = new Random();
        var selected = new HashSet<(int Row, int Column)>();

        // Step 1: Guarantee one clue per row
        for (var row = 0; row < GridSize; row++)
        {
            var column = random.Next(GridSize);
            selected.Add((row, column));
        }

        // Step 2: Guarantee one clue per column (if not already covered)
        for (var column = 0; column < GridSize; column++)
        {
            // Check if this column already has a clue from Step 1
            var hasClue = false;
            for (var row = 0; row < GridSize; row++)
            {
                if (selected.Contains((row, column)))
                {
                    hasClue = true;
                    break;
                }
            }

            // If not, add a random row in this column
            if (!hasClue)
            {
                // Find available rows in this column
                var availableRows = new List<int>();
                for (var row = 0; row < GridSize; row++)
                {
                    if (!selected.Contains((row, column)))
                    {
                        availableRows.Add(row);
                    }
                }

                if (availableRows.Count > 0)
                {
                    var randomRow = availableRows[random.Next(availableRows.Count)];
                    selected.Add((randomRow, column));
                }
            }
        }

        // Step 3: Fill remaining slots randomly until we have exactly 30 clues
        if (selected.Count < PrefilledCellCount)
        {
            var allCoordinates = new List<(int Row, int Column)>();
            for (var row = 0; row < GridSize; row++)
            {
                for (var column = 0; column < GridSize; column++)
                {
                    if (!selected.Contains((row, column)))
                    {
                        allCoordinates.Add((row, column));
                    }
                }
            }

            Shuffle(allCoordinates, random);

            var needed = PrefilledCellCount - selected.Count;
            for (var i = 0; i < needed && i < allCoordinates.Count; i++)
            {
                selected.Add(allCoordinates[i]);
            }
        }

        // Step 4: Place the selected clues in the puzzle
        foreach (var (row, column) in selected)
        {
            var coordinate = new Coordinate(row, column);
            var value = solution.GetCell(coordinate).Value!.Value;
            puzzle.SetPrefilledCell(coordinate, value);
        }

        return puzzle;
    }


    private static void Shuffle<T>(IList<T> list, Random random)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private static int GetPrefilledCellCount(Difficulty difficulty) => difficulty switch
    {
        Difficulty.Easy => 40,
        Difficulty.Medium => 35,
        Difficulty.Hard => 30,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, "Unsupported difficulty level.")
    };

    public (Grid Puzzle, Grid Solution) Generate(Difficulty difficulty)
    {
        var solution = CreateCompleteSolution();
        var puzzle = CreatePuzzle(solution, difficulty);

        return (puzzle, solution);
    }
}
