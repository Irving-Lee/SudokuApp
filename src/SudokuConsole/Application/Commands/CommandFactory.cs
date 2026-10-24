using SudokuConsole.Application.Services;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Commands;

/// <summary>
/// Creates command objects from user input strings.
/// </summary>
public sealed class CommandFactory
{
    private readonly Grid _puzzle;
    private readonly Grid _solution;
    private readonly ISudokuValidator _validator;
    private readonly IHintProvider _hintProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandFactory"/> class.
    /// </summary>
    public CommandFactory(
        Grid puzzle,
        Grid solution,
        ISudokuValidator validator,
        IHintProvider hintProvider)
    {
        _puzzle = puzzle ?? throw new ArgumentNullException(nameof(puzzle));
        _solution = solution ?? throw new ArgumentNullException(nameof(solution));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _hintProvider = hintProvider ?? throw new ArgumentNullException(nameof(hintProvider));
    }

    /// <summary>
    /// Creates a command from the user's input string.
    /// </summary>
    /// <param name="input">The user input (e.g., "A3 4", "hint", "check").</param>
    /// <returns>An executable command.</returns>
    public ICommand CreateCommand(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new InvalidCommand("Invalid command format. Enter command (e.g., A3 4, C5 clear, hint, check, quit).");
        }

        input = input.Trim();

        // Check for simple commands (hint, check, quit)
        if (input.Equals("hint", StringComparison.OrdinalIgnoreCase))
        {
            return new HintCommand(_puzzle, _solution, _hintProvider);
        }

        if (input.Equals("check", StringComparison.OrdinalIgnoreCase))
        {
            return new CheckCommand(_puzzle, _validator);
        }

        if (input.Equals("quit", StringComparison.OrdinalIgnoreCase))
        {
            return new QuitCommand();
        }

        // Parse cell commands (e.g., "A3 4" or "C5 clear")
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
        {
            return new InvalidCommand("Invalid command format. Enter command (e.g., A3 4, C5 clear, hint, check, quit).");
        }

        var cellRef = parts[0];
        var action = parts[1];

        // Parse coordinate
        Coordinate coordinate;
        try
        {
            coordinate = Coordinate.Parse(cellRef);
        }
        catch (ArgumentException ex)
        {
            return new InvalidCommand($"Invalid cell reference: {cellRef}. {ex.Message}");
        }

        // Check if it's a clear command
        if (action.Equals("clear", StringComparison.OrdinalIgnoreCase))
        {
            return new ClearCellCommand(_puzzle, coordinate);
        }

        // Try to parse as a number (1-9)
        if (int.TryParse(action, out var number))
        {
            if (number < 1 || number > 9)
            {
                return new InvalidCommand("Number must be between 1 and 9.");
            }

            return new PlaceNumberCommand(_puzzle, coordinate, number);
        }

        return new InvalidCommand($"Invalid action: {action}. Expected a number (1-9) or 'clear'.");
    }
}
