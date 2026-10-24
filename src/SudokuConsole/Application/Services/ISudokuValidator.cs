using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Services;

/// <summary>
/// Provides validation services for Sudoku grids.
/// </summary>
public interface ISudokuValidator
{
    /// <summary>
    /// Validates the entire grid for Sudoku rule violations.
    /// </summary>
    /// <param name="grid">The grid to validate.</param>
    /// <returns>A validation result indicating success or the first violation found.</returns>
    ValidationResult Validate(Grid grid);
}
