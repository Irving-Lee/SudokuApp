using System.Diagnostics;
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
    private readonly ILeaderboardService _leaderboard;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="GameController"/> class.
    /// </summary>
    public GameController(
        IConsoleDisplay display,
        IPuzzleGenerator puzzleGenerator,
        ISudokuValidator validator,
        IHintProvider hintProvider,
        ILeaderboardService leaderboard)
    {
        _display = display ?? throw new ArgumentNullException(nameof(display));
        _puzzleGenerator = puzzleGenerator ?? throw new ArgumentNullException(nameof(puzzleGenerator));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _hintProvider = hintProvider ?? throw new ArgumentNullException(nameof(hintProvider));
        _leaderboard = leaderboard ?? throw new ArgumentNullException(nameof(leaderboard));
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

        var difficulty = _display.SelectDifficulty();

        var (puzzle, solution) = _puzzleGenerator.Generate(difficulty);
        _display.ShowMessage("Here is your puzzle:");
        _display.DisplayGrid(puzzle);

        var timer = Stopwatch.StartNew();
        var mistakes = 0;
        const int maxMistakes = 5;
        var commandFactory = new CommandFactory(puzzle, solution, _validator, _hintProvider);

        while (true)
        {
            var input = _display.ReadCommand();
            var command = commandFactory.CreateCommand(input);
            var result = command.Execute();

            _display.ShowMessage(result.Message);

            // Track mistakes if check command found violations
            if (command is CheckCommand && !result.Success)
            {
                mistakes++;
                _display.ShowMessage($"Mistakes: {mistakes}/{maxMistakes}");

                if (mistakes >= maxMistakes)
                {
                    _display.ShowMessage("Game Over! Too many mistakes.");
                    return _display.PromptPlayAgain();
                }
            }

            if (result.ShouldRedisplayGrid)
            {
                _display.ShowMessage("Current grid:");
                _display.DisplayGrid(puzzle);
            }

            if (result.ShouldQuit)
            {
                return false;
            }

            if (result.Success && puzzle.IsComplete && _validator.Validate(puzzle).IsValid)
            {
                return CompleteGame(puzzle, difficulty, timer, mistakes);
            }
        }
    }

    private bool CompleteGame(Grid puzzle, Difficulty difficulty, Stopwatch timer, int mistakes)
    {
        _display.DisplayGrid(puzzle);
        _display.ShowWinMessage();

        timer.Stop();
        _display.ShowMessage($"Solved in {timer.Elapsed:mm\\:ss}!");

        if (mistakes > 0)
        {
            _display.ShowMessage($"Mistakes made: {mistakes}");
        }
        else
        {
            _display.ShowMessage("Perfect solve! No mistakes.");
        }

        var playerName = _display.ReadPlayerName();
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player";
        }

        var score = new GameScore
        {
            PlayerName = playerName,
            Difficulty = difficulty,
            SolveTime = timer.Elapsed,
            Mistakes = mistakes,
            CompletedAt = DateTime.Now
        };

        _leaderboard.SaveScore(score);
        _display.ShowMessage("Score saved!");

        var scores = _leaderboard.GetTopScores(difficulty, 100);
        var rank = -1;

        for (var index = 0; index < scores.Count; index++)
        {
            var entry = scores[index];
            if (entry.PlayerName == playerName &&
                entry.SolveTime == score.SolveTime &&
                entry.Mistakes == score.Mistakes)
            {
                rank = index + 1;
                break;
            }
        }

        if (rank > 0 && rank <= 10)
        {
            _display.ShowMessage($"🎉 You placed #{rank} on the {difficulty} leaderboard!");
        }
        else if (rank > 10)
        {
            _display.ShowMessage($"You ranked #{rank} on the {difficulty} leaderboard.");
        }

        var topScores = _leaderboard.GetTopScores(difficulty, 5);
        _display.ShowLeaderboard(topScores, difficulty);

        return _display.PromptPlayAgain();
    }
}