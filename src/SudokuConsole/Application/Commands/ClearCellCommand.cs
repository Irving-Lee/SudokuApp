using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Commands;

/// <summary>
/// Command to clear a cell's value.
/// </summary>
public sealed class ClearCellCommand : ICommand
{
    private readonly Grid _grid;
    private readonly Coordinate _coordinate;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClearCellCommand"/> class.
    /// </summary>
    /// <param name="grid">The game grid.</param>
    /// <param name="coordinate">The target cell coordinate.</param>
    public ClearCellCommand(Grid grid, Coordinate coordinate)
    {
        _grid = grid ?? throw new ArgumentNullException(nameof(grid));
        _coordinate = coordinate;
    }

    /// <inheritdoc />
    public CommandResult Execute()
    {
        try
        {
            _grid.ClearCell(_coordinate);
            return CommandResult.SuccessResult("Cell cleared.");
        }
        catch (InvalidOperationException)
        {
            // Pre-filled cell
            var message = $"Invalid move. {_coordinate} is pre-filled.";
            return CommandResult.FailureResult(message);
        }
    }
}
