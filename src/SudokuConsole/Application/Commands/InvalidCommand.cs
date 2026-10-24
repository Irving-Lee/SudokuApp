namespace SudokuConsole.Application.Commands;

/// <summary>
/// Represents an invalid or unrecognized command.
/// </summary>
public sealed class InvalidCommand : ICommand
{
    private readonly string _errorMessage;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidCommand"/> class.
    /// </summary>
    public InvalidCommand(string errorMessage)
    {
        _errorMessage = errorMessage ?? "Invalid command.";
    }

    /// <inheritdoc />
    public CommandResult Execute() => CommandResult.FailureResult(_errorMessage);
}
