using NUnit.Framework;
using SudokuConsole.Application.Commands;
using SudokuConsole.Application.Services;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Tests.Application.Commands;

[TestFixture]
public class HintCommandTests
{
    [Test]
    public void Execute_WithEmptyCells_ReturnsHint()
    {
        // Arrange
        var generator = new PuzzleGenerator();
        var hintProvider = new HintProvider();
        var (puzzle, solution) = generator.Generate(Difficulty.Hard);

        var command = new HintCommand(puzzle, solution, hintProvider);

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Message, Does.Contain("Hint:"));
        Assert.That(result.Message, Does.Contain("Cell"));
        Assert.That(result.Message, Does.Contain("="));
    }

    [Test]
    public void Execute_NoEmptyCells_ReturnsNoHintMessage()
    {
        // Arrange
        var solution = new Grid();
        var puzzle = new Grid();

        // Fill both completely
        for (var row = 0; row < 9; row++)
        {
            for (var column = 0; column < 9; column++)
            {
                var coord = new Coordinate(row, column);
                var value = (row * 3 + row / 3 + column) % 9 + 1;
                solution.SetCell(coord, value);
                puzzle.SetCell(coord, value);
            }
        }

        var hintProvider = new HintProvider();
        var command = new HintCommand(puzzle, solution, hintProvider);

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Does.Contain("No empty cells"));
    }

    [Test]
    public void Execute_DoesNotModifyGrid()
    {
        // Arrange
        var generator = new PuzzleGenerator();
        var hintProvider = new HintProvider();
        var (puzzle, solution) = generator.Generate(Difficulty.Hard);

        var emptyCountBefore = CountEmptyCells(puzzle);
        var command = new HintCommand(puzzle, solution, hintProvider);

        // Act
        var result = command.Execute();

        // Assert
        var emptyCountAfter = CountEmptyCells(puzzle);
        Assert.That(result.Success, Is.True);
        Assert.That(emptyCountAfter, Is.EqualTo(emptyCountBefore), 
            "HintCommand should not modify the grid");
    }

    private static int CountEmptyCells(Grid grid)
    {
        var count = 0;
        for (var row = 0; row < 9; row++)
        {
            for (var column = 0; column < 9; column++)
            {
                if (grid.GetCell(new Coordinate(row, column)).IsEmpty)
                {
                    count++;
                }
            }
        }
        return count;
    }
}
