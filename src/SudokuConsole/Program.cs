using SudokuConsole.Application.Services;
using SudokuConsole.Infrastructure.UI;
using SudokuConsole.Presentation;

// Create services (manual dependency injection)
var display = new ConsoleDisplay();
var puzzleGenerator = new PuzzleGenerator();
var validator = new SudokuValidator();
var hintProvider = new HintProvider();

// Create and run game controller
var controller = new GameController(display, puzzleGenerator, validator, hintProvider);
controller.Run();
