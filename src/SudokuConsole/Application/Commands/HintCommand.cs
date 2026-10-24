using SudokuConsole.Application.Services;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Commands;

/// <summary>
/// Command that requests a hint without changing the puzzle grid.
/// </summary>
public sealed class HintCommand : ICommand
{
    private readonly Grid _puzzle;
    private readonly Grid _solution;
    private readonly IHintProvider _hintProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="HintCommand"/> class.
    /// </summary>
    public HintCommand(Grid puzzle, Grid solution, IHintProvider hintProvider)
    {
        _puzzle = puzzle ?? throw new ArgumentNullException(nameof(puzzle));
        _solution = solution ?? throw new ArgumentNullException(nameof(solution));
        _hintProvider = hintProvider ?? throw new ArgumentNullException(nameof(hintProvider));
    }

    /// <inheritdoc />
    public CommandResult Execute()
    {
        var hint = _hintProvider.GetHint(_puzzle, _solution);

        if (!hint.Success)
        {
            return CommandResult.FailureResult(
                hint.Message ?? "No hint is available.");
        }

        if (hint.Coordinate is null || hint.Value is null)
        {
            return CommandResult.FailureResult("No hint is available.");
        }

        return CommandResult.SuccessResult(
            $"Hint: Cell {hint.Coordinate.Value} = {hint.Value.Value}");
    }
}
