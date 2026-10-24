using NUnit.Framework;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Tests.Domain;

[TestFixture]
public class GridTests
{
    [Test]
    public void Constructor_Default_Initializes9x9EmptyGrid()
    {
        // Act
        var grid = new Grid();

        // Assert
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                var coordinate = new Coordinate(row, col);
                var cell = grid.GetCell(coordinate);
                
                Assert.That(cell, Is.Not.Null, $"Cell at {coordinate} should not be null");
                Assert.That(cell.IsEmpty, Is.True, $"Cell at {coordinate} should be empty");
                Assert.That(cell.IsPreFilled, Is.False, $"Cell at {coordinate} should not be pre-filled");
            }
        }
    }

    [Test]
    public void Constructor_Default_GridIsNotComplete()
    {
        // Act
        var grid = new Grid();

        // Assert
        Assert.That(grid.IsComplete, Is.False);
    }

    [TestCase(0, 0)]
    [TestCase(4, 4)]
    [TestCase(8, 8)]
    [TestCase(0, 8)]
    [TestCase(8, 0)]
    public void GetCell_ValidCoordinate_ReturnsCell(int row, int col)
    {
        // Arrange
        var grid = new Grid();
        var coordinate = new Coordinate(row, col);

        // Act
        var cell = grid.GetCell(coordinate);

        // Assert
        Assert.That(cell, Is.Not.Null);
        Assert.That(cell.IsEmpty, Is.True);
    }

    [Test]
    public void GetCell_SameCoordinate_ReturnsSameInstance()
    {
        // Arrange
        var grid = new Grid();
        var coordinate = new Coordinate(3, 3);

        // Act
        var cell1 = grid.GetCell(coordinate);
        var cell2 = grid.GetCell(coordinate);

        // Assert
        Assert.That(cell1, Is.SameAs(cell2), "Should return the same cell instance");
    }

    [TestCase(0, 0, 5)]
    [TestCase(4, 4, 1)]
    [TestCase(8, 8, 9)]
    public void SetCell_ValidValue_UpdatesCell(int row, int col, int value)
    {
        // Arrange
        var grid = new Grid();
        var coordinate = new Coordinate(row, col);

        // Act
        grid.SetCell(coordinate, value);

        // Assert
        var cell = grid.GetCell(coordinate);
        Assert.That(cell.Value, Is.EqualTo(value));
        Assert.That(cell.IsEmpty, Is.False);
    }

    [Test]
    public void SetCell_MultipleValues_UpdatesCorrectCells()
    {
        // Arrange
        var grid = new Grid();

        // Act
        grid.SetCell(new Coordinate(0, 0), 1);
        grid.SetCell(new Coordinate(1, 1), 2);
        grid.SetCell(new Coordinate(8, 8), 9);

        // Assert
        Assert.That(grid.GetCell(new Coordinate(0, 0)).Value, Is.EqualTo(1));
        Assert.That(grid.GetCell(new Coordinate(1, 1)).Value, Is.EqualTo(2));
        Assert.That(grid.GetCell(new Coordinate(8, 8)).Value, Is.EqualTo(9));
        Assert.That(grid.GetCell(new Coordinate(0, 1)).IsEmpty, Is.True, "Adjacent cell should remain empty");
    }

    [TestCase(0)]
    [TestCase(10)]
    [TestCase(-1)]
    public void SetCell_InvalidValue_ThrowsArgumentException(int invalidValue)
    {
        // Arrange
        var grid = new Grid();
        var coordinate = new Coordinate(0, 0);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => grid.SetCell(coordinate, invalidValue));
    }

    [Test]
    public void ClearCell_NonPreFilledCell_ClearsValue()
    {
        // Arrange
        var grid = new Grid();
        var coordinate = new Coordinate(0, 0);
        grid.SetCell(coordinate, 5);

        // Act
        grid.ClearCell(coordinate);

        // Assert
        var cell = grid.GetCell(coordinate);
        Assert.That(cell.IsEmpty, Is.True);
        Assert.That(cell.Value, Is.Null);
    }

    [Test]
    public void ClearCell_EmptyCell_RemainsEmpty()
    {
        // Arrange
        var grid = new Grid();
        var coordinate = new Coordinate(0, 0);

        // Act
        grid.ClearCell(coordinate);

        // Assert
        var cell = grid.GetCell(coordinate);
        Assert.That(cell.IsEmpty, Is.True);
    }

    [Test]
    public void IsComplete_PartiallyFilledGrid_ReturnsFalse()
    {
        // Arrange
        var grid = new Grid();
        
        // Fill only first row
        for (int col = 0; col < 9; col++)
        {
            grid.SetCell(new Coordinate(0, col), col + 1);
        }

        // Act & Assert
        Assert.That(grid.IsComplete, Is.False);
    }

    [Test]
    public void IsComplete_FullyFilledGrid_ReturnsTrue()
    {
        // Arrange
        var grid = new Grid();
        
        // Fill entire grid with value 1 (not a valid Sudoku, but tests IsComplete)
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                grid.SetCell(new Coordinate(row, col), 1);
            }
        }

        // Act & Assert
        Assert.That(grid.IsComplete, Is.True);
    }

    [Test]
    public void IsComplete_AlmostFullGrid_ReturnsFalse()
    {
        // Arrange
        var grid = new Grid();
        
        // Fill entire grid except one cell
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (row == 8 && col == 8) continue; // Skip last cell
                grid.SetCell(new Coordinate(row, col), 1);
            }
        }

        // Act & Assert
        Assert.That(grid.IsComplete, Is.False);
    }

    [Test]
    public void GetCell_AfterSetCell_ReturnsSameCellWithNewValue()
    {
        // Arrange
        var grid = new Grid();
        var coordinate = new Coordinate(2, 3);
        var cellBefore = grid.GetCell(coordinate);

        // Act
        grid.SetCell(coordinate, 7);
        var cellAfter = grid.GetCell(coordinate);

        // Assert
        Assert.That(cellBefore, Is.SameAs(cellAfter), "Should be the same cell instance");
        Assert.That(cellAfter.Value, Is.EqualTo(7));
    }

}
