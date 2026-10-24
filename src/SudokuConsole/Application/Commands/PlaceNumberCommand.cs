using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Commands;

/// <summary>
/// Command to place a number in a specific cell.
/// </summary>
public sealed class PlaceNumberCommand : ICommand
{
    private readonly Grid _grid;
    private readonly Coordinate _coordinate;
    private readonly int _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaceNumberCommand"/> class.
    /// </summary>
    /// <param name="grid">The game grid.</param>
    /// <param name="coordinate">The target cell coordinate.</param>
    /// <param name="value">The number to place (1-9).</param>
    public PlaceNumberCommand(Grid grid, Coordinate coordinate, int value)
    {
        _grid = grid ?? throw new ArgumentNullException(nameof(grid));
        _coordinate = coordinate;
        _value = value;
    }

    /// <inheritdoc />
    public CommandResult Execute()
    {
        try
        {
            _grid.SetCell(_coordinate, _value);
            return CommandResult.SuccessResult("Move accepted.");
        }
        catch (ArgumentException ex)
        {
            // Invalid value (not 1-9)
            return CommandResult.FailureResult(ex.Message);
        }
        catch (InvalidOperationException)
        {
            // Pre-filled cell
            var message = $"Invalid move. {_coordinate} is pre-filled.";
            return CommandResult.FailureResult(message);
        }
    }
}
