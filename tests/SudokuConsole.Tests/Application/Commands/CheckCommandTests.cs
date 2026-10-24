using NUnit.Framework;
using SudokuConsole.Application.Commands;
using SudokuConsole.Application.Services;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Tests.Application.Commands;

[TestFixture]
public class CheckCommandTests
{
    [Test]
    public void Execute_ValidGrid_ReturnsNoViolations()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCell(new Coordinate(0, 0), 5);
        grid.SetCell(new Coordinate(0, 1), 3);
        grid.SetCell(new Coordinate(1, 0), 6);

        var validator = new SudokuValidator();
        var command = new CheckCommand(grid, validator);

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Message, Does.Contain("No rule violations detected"));
    }

    [Test]
    public void Execute_RowViolation_ReturnsFailure()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCell(new Coordinate(0, 0), 5);
        grid.SetCell(new Coordinate(0, 1), 3);
        grid.SetCell(new Coordinate(0, 2), 5); // Duplicate 5 in row A

        var validator = new SudokuValidator();
        var command = new CheckCommand(grid, validator);

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Does.Contain("5"));
        Assert.That(result.Message, Does.Contain("Row A"));
    }

    [Test]
    public void Execute_ColumnViolation_ReturnsFailure()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCell(new Coordinate(0, 0), 5);
        grid.SetCell(new Coordinate(1, 0), 3);
        grid.SetCell(new Coordinate(2, 0), 5); // Duplicate 5 in column 1

        var validator = new SudokuValidator();
        var command = new CheckCommand(grid, validator);

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Does.Contain("5"));
        Assert.That(result.Message, Does.Contain("Column 1"));
    }

    [Test]
    public void Execute_SubgridViolation_ReturnsFailure()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCell(new Coordinate(0, 0), 8);
        grid.SetCell(new Coordinate(1, 1), 8); // Duplicate 8 in same subgrid

        var validator = new SudokuValidator();
        var command = new CheckCommand(grid, validator);

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Does.Contain("8"));
        Assert.That(result.Message, Does.Contain("3×3 subgrid"));
    }
}
