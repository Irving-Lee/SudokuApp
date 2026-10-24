using SudokuConsole.Application.Services;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Commands;

/// <summary>
/// Command to validate the current grid state against Sudoku rules.
/// </summary>
public sealed class CheckCommand : ICommand
{
    private readonly Grid _grid;
    private readonly ISudokuValidator _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CheckCommand"/> class.
    /// </summary>
    public CheckCommand(Grid grid, ISudokuValidator validator)
    {
        _grid = grid ?? throw new ArgumentNullException(nameof(grid));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    /// <inheritdoc />
    public CommandResult Execute()
    {
        var validationResult = _validator.Validate(_grid);

        if (validationResult.IsValid)
        {
            return CommandResult.SuccessResult(
                "No rule violations detected.", 
                redisplayGrid: false);
        }

        return CommandResult.FailureResult(
            validationResult.ViolationMessage ?? "Validation failed.",
            redisplayGrid: false);
    }
}
