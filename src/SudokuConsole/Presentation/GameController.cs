using SudokuConsole.Application.Commands;
using SudokuConsole.Application.Services;
using SudokuConsole.Domain.Models;
using SudokuConsole.Infrastructure.UI;

namespace SudokuConsole.Presentation;

/// <summary>
/// Controls the main game loop and coordinates between UI and game logic.
/// </summary>
public sealed class GameController
{
    private readonly IConsoleDisplay _display;
    private readonly IPuzzleGenerator _puzzleGenerator;
    private readonly ISudokuValidator _validator;
    private readonly IHintProvider _hintProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameController"/> class.
    /// </summary>
    public GameController(
        IConsoleDisplay display,
        IPuzzleGenerator puzzleGenerator,
        ISudokuValidator validator,
        IHintProvider hintProvider)
    {
        _display = display ?? throw new ArgumentNullException(nameof(display));
        _puzzleGenerator = puzzleGenerator ?? throw new ArgumentNullException(nameof(puzzleGenerator));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _hintProvider = hintProvider ?? throw new ArgumentNullException(nameof(hintProvider));
    }

    /// <summary>
    /// Runs the main game loop.
    /// </summary>
    public void Run()
    {
        var playAgain = true;

        while (playAgain)
        {
            playAgain = PlayGame();
        }
    }

    private bool PlayGame()
    {
        _display.ShowWelcome();

        // Let user select difficulty
        var difficulty = _display.SelectDifficulty();

        // Generate puzzle
        var (puzzle, solution) = _puzzleGenerator.Generate(difficulty);
        _display.ShowMessage("Here is your puzzle:");
        _display.DisplayGrid(puzzle);

        var commandFactory = new CommandFactory(puzzle, solution, _validator, _hintProvider);

        // Game loop
        while (true)
        {
            var input = _display.ReadCommand();
            var command = commandFactory.CreateCommand(input);
            var result = command.Execute();

            // Display message
            _display.ShowMessage(result.Message);

            // Redisplay grid if needed
            if (result.ShouldRedisplayGrid)
            {
                _display.ShowMessage("Current grid:");
                _display.DisplayGrid(puzzle);
            }

            // Check for quit
            if (result.ShouldQuit)
            {
                return false; // Don't play again
            }

            // Check for win condition
            if (result.Success && puzzle.IsComplete)
            {
                var validationResult = _validator.Validate(puzzle);
                if (validationResult.IsValid)
                {
                    _display.DisplayGrid(puzzle);
                    _display.ShowWinMessage();
                    return _display.PromptPlayAgain();
                }
            }
        }
    }
}
