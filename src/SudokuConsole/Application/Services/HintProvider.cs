using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Services;

/// <summary>
/// Provides a correct value for an empty cell using the puzzle solution.
/// </summary>
public sealed class HintProvider : IHintProvider
{
    private const int GridSize = 9;

    /// <inheritdoc />
    public HintResult GetHint(Grid puzzle, Grid solution)
    {
        ArgumentNullException.ThrowIfNull(puzzle);
        ArgumentNullException.ThrowIfNull(solution);

        var emptyCells = new List<Coordinate>();

        for (var row = 0; row < GridSize; row++)
        {
            for (var column = 0; column < GridSize; column++)
            {
                var coordinate = new Coordinate(row, column);

                if (puzzle.GetCell(coordinate).IsEmpty)
                {
                    emptyCells.Add(coordinate);
                }
            }
        }

        if (emptyCells.Count == 0)
        {
            return HintResult.NoHintAvailable("No empty cells are available for a hint.");
        }

        var selected = emptyCells[Random.Shared.Next(emptyCells.Count)];
        var solutionValue = solution.GetCell(selected).Value;

        if (solutionValue is null)
        {
            throw new ArgumentException(
                $"The solution has no value for cell {selected}.",
                nameof(solution));
        }

        return HintResult.SuccessfulHint(selected, solutionValue.Value);
    }
}
