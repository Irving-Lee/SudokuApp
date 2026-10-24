using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Services;

/// <summary>
/// Provides hints for solving a Sudoku puzzle.
/// </summary>
public interface IHintProvider
{
    /// <summary>
    /// Provides a hint by revealing the correct value for an empty cell.
    /// </summary>
    /// <param name="puzzle">The current puzzle grid.</param>
    /// <param name="solution">The complete solution grid.</param>
    /// <returns>A hint result containing the coordinate and value, or a message if no hint is available.</returns>
    HintResult GetHint(Grid puzzle, Grid solution);
}
