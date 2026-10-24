using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Services;

/// <summary>
/// Represents the result of a hint request.
/// </summary>
public record HintResult
{
    /// <summary>
    /// Gets a value indicating whether a hint was successfully provided.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Gets the coordinate of the hint cell, if available.
    /// </summary>
    public Coordinate? Coordinate { get; init; }

    /// <summary>
    /// Gets the correct value for the hint cell, if available.
    /// </summary>
    public int? Value { get; init; }

    /// <summary>
    /// Gets a message (e.g., when no hint is available).
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Creates a successful hint result.
    /// </summary>
    public static HintResult SuccessfulHint(Coordinate coordinate, int value) => new()
    {
        Success = true,
        Coordinate = coordinate,
        Value = value
    };

    /// <summary>
    /// Creates a failure result (e.g., no empty cells).
    /// </summary>
    public static HintResult NoHintAvailable(string message) => new()
    {
        Success = false,
        Message = message
    };
}
