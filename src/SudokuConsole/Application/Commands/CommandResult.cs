namespace SudokuConsole.Application.Commands;

/// <summary>
/// Represents the result of executing a command.
/// </summary>
public record CommandResult
{
    /// <summary>
    /// Gets a value indicating whether the command executed successfully.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Gets the message to display to the user.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the game should quit.
    /// </summary>
    public bool ShouldQuit { get; init; }

    /// <summary>
    /// Gets a value indicating whether the grid should be redisplayed.
    /// </summary>
    public bool ShouldRedisplayGrid { get; init; } = true;

    /// <summary>
    /// Creates a successful command result.
    /// </summary>
    public static CommandResult SuccessResult(string message, bool redisplayGrid = true) => new()
    {
        Success = true,
        Message = message,
        ShouldRedisplayGrid = redisplayGrid
    };

    /// <summary>
    /// Creates a failure command result.
    /// </summary>
    public static CommandResult FailureResult(string message, bool redisplayGrid = true) => new()
    {
        Success = false,
        Message = message,
        ShouldRedisplayGrid = redisplayGrid
    };

    /// <summary>
    /// Creates a quit command result.
    /// </summary>
    public static CommandResult QuitResult() => new()
    {
        Success = true,
        Message = "Goodbye!",
        ShouldQuit = true,
        ShouldRedisplayGrid = false
    };
}
