using SudokuConsole.Domain.Models;

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
}
