namespace SudokuConsole.Application.Commands;

/// <summary>
/// Represents a user command that can be executed.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <returns>The result of the command execution.</returns>
    CommandResult Execute();
}
