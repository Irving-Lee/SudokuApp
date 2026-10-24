using SudokuConsole.Domain.Models;

namespace SudokuConsole.Infrastructure.UI;

/// <summary>
/// Provides console display operations for the Sudoku game.
/// </summary>
public interface IConsoleDisplay
{
    /// <summary>
    /// Displays a welcome message.
    /// </summary>
    void ShowWelcome();

    /// <summary>
    /// Displays the Sudoku grid.
    /// </summary>
    /// <param name="grid">The grid to display.</param>
    void DisplayGrid(Grid grid);

    /// <summary>
    /// Displays a message to the user.
    /// </summary>
    /// <param name="message">The message to display.</param>
    void ShowMessage(string message);

    /// <summary>
    /// Displays the command prompt and reads user input.
    /// </summary>
    /// <returns>The user's input string.</returns>
    string ReadCommand();

    /// <summary>
    /// Displays a win message.
    /// </summary>
    void ShowWinMessage();

    /// <summary>
    /// Prompts the user to play again and reads their response.
    /// </summary>
    /// <returns>True if the user wants to play again, false otherwise.</returns>
    bool PromptPlayAgain();
}
