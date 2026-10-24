using SudokuConsole.Domain.Models;
using SudokuConsole.Application.Services;

namespace SudokuConsole.Infrastructure.UI;

/// <summary>
/// Console-based display implementation for the Sudoku game.
/// </summary>
public sealed class ConsoleDisplay : IConsoleDisplay
{
    /// <inheritdoc />
    public void ShowWelcome()
    {
        Console.WriteLine("Welcome to Sudoku!");
        Console.WriteLine();
    }

    /// <inheritdoc />
    public void DisplayGrid(Grid grid)
    {
        Console.WriteLine("    1 2 3 4 5 6 7 8 9");

        for (var row = 0; row < 9; row++)
        {
            var rowLetter = (char)('A' + row);
            Console.Write($"  {rowLetter} ");

            for (var column = 0; column < 9; column++)
            {
                var cell = grid.GetCell(new Coordinate(row, column));
                var display = cell.IsEmpty ? "_" : cell.Value.ToString();
                Console.Write(display);

                if (column < 8)
                {
                    Console.Write(" ");
                }
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    /// <inheritdoc />
    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
        Console.WriteLine();
    }

    /// <inheritdoc />
    public string ReadCommand()
    {
        Console.WriteLine("Enter command (e.g., A3 4, C5 clear, hint, check, quit):");
        return Console.ReadLine() ?? string.Empty;
    }

    /// <inheritdoc />
    public void ShowWinMessage()
    {
        Console.WriteLine();
        Console.WriteLine("You have successfully completed the Sudoku puzzle!");
    }

    /// <inheritdoc />
    public bool PromptPlayAgain()
    {
        Console.WriteLine("Press any key to play again...");
        Console.ReadKey(intercept: true);
        Console.Clear();
        return true;
    }

    /// <inheritdoc />
    public Difficulty SelectDifficulty()
    {
        Console.WriteLine("Select difficulty:");
        Console.WriteLine("1. Easy (40 clues)");
        Console.WriteLine("2. Medium (35 clues)");
        Console.WriteLine("3. Hard (30 clues)");
        Console.WriteLine();
        Console.Write("Enter choice (1-3): ");

        while (true)
        {
            var input = Console.ReadLine()?.Trim().ToLowerInvariant();

            switch (input)
            {
                case "1":
                case "easy":
                    return Difficulty.Easy;

                case "2":
                case "medium":
                    return Difficulty.Medium;

                case "3":
                case "hard":
                    return Difficulty.Hard;

                default:
                    Console.Write("Invalid choice. Enter 1, 2, or 3: ");
                    break;
            }
        }
    }

}
