using SudokuConsole.Application.Services;
using SudokuConsole.Infrastructure.UI;
using SudokuConsole.Presentation;

var display = new ConsoleDisplay();
var puzzleGenerator = new PuzzleGenerator();
var validator = new SudokuValidator();
var hintProvider = new HintProvider();
var leaderboard = new LeaderboardService();

var controller = new GameController(
    display,
    puzzleGenerator,
    validator,
    hintProvider,
    leaderboard);

controller.Run();