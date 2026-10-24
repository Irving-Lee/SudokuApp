using NUnit.Framework;
using SudokuConsole.Application.Commands;
using SudokuConsole.Application.Services;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Tests.Application.Commands;

[TestFixture]
public class CommandFactoryTests
{
    private Grid _puzzle = null!;
    private Grid _solution = null!;
    private ISudokuValidator _validator = null!;
    private IHintProvider _hintProvider = null!;
    private CommandFactory _factory = null!;

    [SetUp]
    public void SetUp()
    {
        var generator = new PuzzleGenerator();
        (_puzzle, _solution) = generator.Generate();
        _validator = new SudokuValidator();
        _hintProvider = new HintProvider();
        _factory = new CommandFactory(_puzzle, _solution, _validator, _hintProvider);
    }

    [TestCase("A3 4")]
    [TestCase("a3 4")]
    [TestCase("I9 1")]
    [TestCase("E5 7")]
    public void CreateCommand_ValidPlaceNumber_ReturnsPlaceNumberCommand(string input)
    {
        // Act
        var command = _factory.CreateCommand(input);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command, Is.InstanceOf<PlaceNumberCommand>());
    }

    [TestCase("A3 clear")]
    [TestCase("a3 clear")]
    [TestCase("I9 CLEAR")]
    [TestCase("E5 Clear")]
    public void CreateCommand_ValidClear_ReturnsClearCellCommand(string input)
    {
        // Act
        var command = _factory.CreateCommand(input);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command, Is.InstanceOf<ClearCellCommand>());
    }

    [TestCase("hint")]
    [TestCase("HINT")]
    [TestCase("Hint")]
    [TestCase("  hint  ")]
    public void CreateCommand_ValidHint_ReturnsHintCommand(string input)
    {
        // Act
        var command = _factory.CreateCommand(input);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command, Is.InstanceOf<HintCommand>());
    }

    [TestCase("check")]
    [TestCase("CHECK")]
    [TestCase("Check")]
    [TestCase("  check  ")]
    public void CreateCommand_ValidCheck_ReturnsCheckCommand(string input)
    {
        // Act
        var command = _factory.CreateCommand(input);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command, Is.InstanceOf<CheckCommand>());
    }

    [TestCase("quit")]
    [TestCase("QUIT")]
    [TestCase("Quit")]
    [TestCase("  quit  ")]
    public void CreateCommand_ValidQuit_ReturnsQuitCommand(string input)
    {
        // Act
        var command = _factory.CreateCommand(input);

        // Assert
        Assert.That(command, Is.Not.Null);
        Assert.That(command, Is.InstanceOf<QuitCommand>());
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase("invalid")]
    [TestCase("A")]
    [TestCase("A3")]
    [TestCase("A10 5")]
    [TestCase("Z1 5")]
    public void CreateCommand_InvalidInput_ReturnsInvalidCommand(string input)
    {
        // Act
        var command = _factory.CreateCommand(input);

        // Assert
        Assert.That(command, Is.Not.Null);
        var result = command.Execute();
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Does.Contain("Invalid").Or.Contains("format").IgnoreCase);
    }
}
