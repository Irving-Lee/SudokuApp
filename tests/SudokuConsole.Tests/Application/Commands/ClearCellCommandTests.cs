using NUnit.Framework;
using SudokuConsole.Application.Commands;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Tests.Application.Commands;

[TestFixture]
public class ClearCellCommandTests
{
    [Test]
    public void Execute_NonPreFilledCell_ClearsCell()
    {
        // Arrange
        var grid = new Grid();
        var coordinate = new Coordinate(0, 0);
        grid.SetCell(coordinate, 5);

        var command = new ClearCellCommand(grid, coordinate);

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Message, Does.Contain("Cell cleared"));
        Assert.That(grid.GetCell(coordinate).IsEmpty, Is.True);
    }

    [Test]
    public void Execute_EmptyCell_SucceedsWithMessage()
    {
        // Arrange
        var grid = new Grid();
        var coordinate = new Coordinate(0, 0);
        // Cell is already empty

        var command = new ClearCellCommand(grid, coordinate);

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(grid.GetCell(coordinate).IsEmpty, Is.True);
    }

    [Test]
    public void Execute_PreFilledCell_ReturnsFailure()
    {
        // Arrange
        var grid = new Grid();
        var coordinate = new Coordinate(0, 0);
        grid.SetPrefilledCell(coordinate, 3);

        var command = new ClearCellCommand(grid, coordinate);

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Does.Contain("A1 is pre-filled"));
        Assert.That(grid.GetCell(coordinate).Value, Is.EqualTo(3), "Value should remain unchanged");
    }
}
