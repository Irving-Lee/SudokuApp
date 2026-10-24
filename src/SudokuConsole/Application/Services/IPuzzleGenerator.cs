using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Services;

/// <summary>
/// Creates a Sudoku puzzle and its completed solution.
/// </summary>
public interface IPuzzleGenerator
{
    /// <summary>
    /// Generates a puzzle with exactly 30 pre-filled cells and its complete solution.
    /// </summary>
    (Grid Puzzle, Grid Solution) Generate(Difficulty difficulty);
}
