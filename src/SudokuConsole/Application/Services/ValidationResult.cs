using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Services;

/// <summary>
/// Represents the result of a Sudoku validation check.
/// </summary>
public record ValidationResult
{
    /// <summary>
    /// Gets a value indicating whether the grid is valid.
    /// </summary>
    public bool IsValid { get; init; }

    /// <summary>
    /// Gets the violation message if validation failed, or null if valid.
    /// </summary>
    public string? ViolationMessage { get; init; }

    /// <summary>
    /// Creates a successful validation result.
    /// </summary>
    public static ValidationResult Success() => new() { IsValid = true };

    /// <summary>
    /// Creates a failed validation result with a violation message.
    /// </summary>
    public static ValidationResult Failure(string message) => new() 
    { 
        IsValid = false, 
        ViolationMessage = message 
    };
}
