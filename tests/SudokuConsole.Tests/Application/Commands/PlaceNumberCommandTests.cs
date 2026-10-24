using NUnit.Framework;
using SudokuConsole.Application.Commands;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Tests.Application.Commands;

[TestFixture]
public class PlaceNumberCommandTests
{
    [Test]
    public void Execute_ValidMove_PlacesNumberInGrid()
    {
        // Arrange
        var grid = new Grid();
        var coordinate = new Coordinate(0, 0);
        var command = new PlaceNumberCommand(grid, coordinate, 5);

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Message, Does.Contain("Move accepted"));
        Assert.That(grid.GetCell(coordinate).Value, Is.EqualTo(5));
    }

    [Test]
    public void Execute_PreFilledCell_ReturnsFailure()
    {
        // Arrange
        var grid = new Grid();
        var coordinate = new Coordinate(0, 0);
        grid.SetPrefilledCell(coordinate, 3);

        var command = new PlaceNumberCommand(grid, coordinate, 5);

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Does.Contain("A1 is pre-filled"));
        Assert.That(grid.GetCell(coordinate).Value, Is.EqualTo(3), "Value should remain unchanged");
    }

    [TestCase(0)]
    [TestCase(10)]
    [TestCase(-1)]
    public void Execute_InvalidNumber_ReturnsFailure(int invalidNumber)
    {
        // Arrange
        var grid = new Grid();
        var coordinate = new Coordinate(0, 0);
        var command = new PlaceNumberCommand(grid, coordinate, invalidNumber);

        // Act
        var result = command.Execute();

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Does.Contain("between 1 and 9"));
    }
}
