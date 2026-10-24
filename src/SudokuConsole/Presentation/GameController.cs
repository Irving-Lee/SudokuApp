using SudokuConsole.Application.Commands;
using SudokuConsole.Application.Services;
using SudokuConsole.Infrastructure.UI;
using System.Diagnostics;

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

        // Let user select difficulty
        var difficulty = _display.SelectDifficulty();

        // Generate puzzle
        var (puzzle, solution) = _puzzleGenerator.Generate(difficulty);
        _display.ShowMessage("Here is your puzzle:");
        _display.DisplayGrid(puzzle);
        // Start timer
        var timer = Stopwatch.StartNew();
        var mistakes = 0;
        const int maxMistakes = 5;
        var commandFactory = new CommandFactory(puzzle, solution, _validator, _hintProvider);

        // Game loop
        while (true)
        {
            var input = _display.ReadCommand();
            var command = commandFactory.CreateCommand(input);
            var result = command.Execute();

            // Display message
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
                    
                    // Stop timer and display elapsed time
                    timer.Stop();
                    _display.ShowMessage($"Solved in {timer.Elapsed:mm\\:ss}!");
                    
                    // Display mistake stats
                    if (mistakes > 0)
                    {
                        _display.ShowMessage($"Mistakes made: {mistakes}");
                    }
                    else
                    {
                        _display.ShowMessage("Perfect solve! No mistakes.");
                    }
                    
                    // Ask for player name
                    Console.Write("Enter your name: ");
                    var playerName = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(playerName))
                    {
                        playerName = "Player";
                    }
                    
                    // Save score
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
                    
                    // Calculate and show rank
                    var allScores = _leaderboard.GetTopScores(difficulty, 100);
                    var yourRank = -1;
                    
                    for (int i = 0; i < allScores.Count; i++)
                    {
                        var s = allScores[i];
                        if (s.PlayerName == playerName && 
                            s.SolveTime == score.SolveTime && 
                            s.Mistakes == score.Mistakes)
                        {
                            yourRank = i + 1;
                            break;
                        }
                    }
                    
                    if (yourRank > 0 && yourRank <= 10)
                    {
                        _display.ShowMessage($"🎉 You placed #{yourRank} on the {difficulty} leaderboard!");
                    }
                    else if (yourRank > 10)
                    {
                        _display.ShowMessage($"You ranked #{yourRank} on the {difficulty} leaderboard.");
                    }
                    
                    // Show top 5
                    var topScores = _leaderboard.GetTopScores(difficulty, 5);
                    _display.ShowLeaderboard(topScores, difficulty);
                    
                     return _display.PromptPlayAgain();
                }
            }
        }
    }
}