using NUnit.Framework;
using SudokuConsole.Application.Services;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Tests.Application.Services;

[TestFixture]
public class SudokuValidatorTests
{
    private ISudokuValidator _validator = null!;

    [SetUp]
    public void SetUp()
    {
        _validator = new SudokuValidator();
    }

    [Test]
    public void Validate_EmptyGrid_ReturnsValid()
    {
        // Arrange
        var grid = new Grid();

        // Act
        var result = _validator.Validate(grid);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.ViolationMessage, Is.Null);
    }

    [Test]
    public void Validate_ValidPartialGrid_ReturnsValid()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCell(new Coordinate(0, 0), 5);
        grid.SetCell(new Coordinate(0, 1), 3);
        grid.SetCell(new Coordinate(1, 0), 6);

        // Act
        var result = _validator.Validate(grid);

        // Assert
        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void Validate_DuplicateInRow_ReturnsFailure()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCell(new Coordinate(0, 0), 5);
        grid.SetCell(new Coordinate(0, 1), 3);
        grid.SetCell(new Coordinate(0, 2), 5); // Duplicate 5 in row A

        // Act
        var result = _validator.Validate(grid);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.ViolationMessage, Does.Contain("5"));
        Assert.That(result.ViolationMessage, Does.Contain("Row A"));
    }

    [Test]
    public void Validate_DuplicateInColumn_ReturnsFailure()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCell(new Coordinate(0, 0), 5);
        grid.SetCell(new Coordinate(1, 0), 3);
        grid.SetCell(new Coordinate(2, 0), 5); // Duplicate 5 in column 1

        // Act
        var result = _validator.Validate(grid);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.ViolationMessage, Does.Contain("5"));
        Assert.That(result.ViolationMessage, Does.Contain("Column 1"));
    }

    [Test]
    public void Validate_DuplicateInDifferentColumn_ReturnsFailure()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCell(new Coordinate(0, 5), 7);
        grid.SetCell(new Coordinate(3, 5), 7); // Duplicate 7 in column 6

        // Act
        var result = _validator.Validate(grid);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.ViolationMessage, Does.Contain("7"));
        Assert.That(result.ViolationMessage, Does.Contain("Column 6"));
    }

    [Test]
    public void Validate_DuplicateInSubgrid_ReturnsFailure()
    {
        // Arrange
        var grid = new Grid();
        // Top-left subgrid (rows 0-2, cols 0-2)
        grid.SetCell(new Coordinate(0, 0), 5);
        grid.SetCell(new Coordinate(1, 1), 3);
        grid.SetCell(new Coordinate(2, 2), 5); // Duplicate 5 in same subgrid

        // Act
        var result = _validator.Validate(grid);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.ViolationMessage, Does.Contain("5"));
        Assert.That(result.ViolationMessage, Does.Contain("3×3 subgrid"));
    }

    [Test]
    public void Validate_DuplicateInMiddleSubgrid_ReturnsFailure()
    {
        // Arrange
        var grid = new Grid();
        // Middle subgrid (rows 3-5, cols 3-5)
        grid.SetCell(new Coordinate(3, 3), 8);
        grid.SetCell(new Coordinate(4, 4), 8); // Duplicate 8 in same subgrid

        // Act
        var result = _validator.Validate(grid);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.ViolationMessage, Does.Contain("8"));
        Assert.That(result.ViolationMessage, Does.Contain("3×3 subgrid"));
    }

    [Test]
    public void Validate_SameNumberInDifferentSubgrids_ReturnsValid()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCell(new Coordinate(0, 0), 5); // Top-left subgrid
        grid.SetCell(new Coordinate(3, 3), 5); // Middle subgrid
        grid.SetCell(new Coordinate(6, 6), 5); // Bottom-right subgrid

        // Act
        var result = _validator.Validate(grid);

        // Assert
        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void Validate_RowViolationTakesPriorityOverColumn()
    {
        // Arrange - Create both row and column violations
        var grid = new Grid();
        grid.SetCell(new Coordinate(0, 0), 5);
        grid.SetCell(new Coordinate(0, 1), 5); // Row violation
        grid.SetCell(new Coordinate(1, 0), 5); // Also creates column violation

        // Act
        var result = _validator.Validate(grid);

        // Assert - Should report row violation first
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.ViolationMessage, Does.Contain("Row"));
    }

    [Test]
    public void Validate_ColumnViolationTakesPriorityOverSubgrid()
    {
        // Arrange - Create both column and subgrid violations
        var grid = new Grid();
        grid.SetCell(new Coordinate(0, 0), 5);
        grid.SetCell(new Coordinate(3, 0), 5); // Column violation (different subgrids)
        grid.SetCell(new Coordinate(1, 1), 5); // Also in same subgrid as first

        // Act
        var result = _validator.Validate(grid);

        // Assert - Should report column violation first
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.ViolationMessage, Does.Contain("Column"));
    }

    [Test]
    public void Validate_CompleteValidGrid_ReturnsValid()
    {
        // Arrange - Create a valid complete Sudoku (just first row for now)
        var grid = new Grid();
        for (int col = 0; col < 9; col++)
        {
            grid.SetCell(new Coordinate(0, col), col + 1);
        }

        // Act
        var result = _validator.Validate(grid);

        // Assert
        Assert.That(result.IsValid, Is.True);
    }

}
