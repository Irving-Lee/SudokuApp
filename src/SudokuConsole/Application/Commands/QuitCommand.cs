namespace SudokuConsole.Application.Commands;

/// <summary>
/// Command that requests the game to quit.
/// </summary>
public sealed class QuitCommand : ICommand
{
    /// <inheritdoc />
    public CommandResult Execute() => CommandResult.QuitResult();
}
