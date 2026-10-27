using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Services;

/// <summary>
/// Creates a Sudoku puzzle and its completed solution.
/// </summary>
public interface IPuzzleGenerator
{
    /// <summary>
    /// Generates a puzzle with 40, 35, or 30 clues, depending on difficulty, and its complete solution.
    /// </summary>
    (Grid Puzzle, Grid Solution) Generate(Difficulty difficulty);
}
