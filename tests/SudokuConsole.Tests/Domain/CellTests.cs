using NUnit.Framework;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Tests.Domain;

[TestFixture]
public class CellTests
{
    [Test]
    public void Constructor_Default_CreatesEmptyCell()
    {
        // Act
        var cell = new Cell();

        // Assert
        Assert.That(cell.Value, Is.Null);
        Assert.That(cell.IsEmpty, Is.True);
        Assert.That(cell.IsPreFilled, Is.False);
    }

    [TestCase(1)]
    [TestCase(5)]
    [TestCase(9)]
    public void Constructor_WithValidValue_StoresValue(int value)
    {
        // Act
        var cell = new Cell(value);

        // Assert
        Assert.That(cell.Value, Is.EqualTo(value));
        Assert.That(cell.IsEmpty, Is.False);
        Assert.That(cell.IsPreFilled, Is.False);
    }

    [Test]
    public void Constructor_WithValueAndPreFilled_StoresCorrectly()
    {
        // Act
        var cell = new Cell(7, isPreFilled: true);

        // Assert
        Assert.That(cell.Value, Is.EqualTo(7));
        Assert.That(cell.IsEmpty, Is.False);
        Assert.That(cell.IsPreFilled, Is.True);
    }

    [TestCase(0)]
    [TestCase(10)]
    [TestCase(-1)]
    [TestCase(100)]
    public void Constructor_InvalidValue_ThrowsArgumentException(int invalidValue)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Cell(invalidValue));
        Assert.That(ex.Message, Does.Contain("Value must be between 1 and 9"));
    }

    [TestCase(1)]
    [TestCase(5)]
    [TestCase(9)]
    public void SetValue_ValidValue_UpdatesCell(int value)
    {
        // Arrange
        var cell = new Cell();

        // Act
        cell.SetValue(value);

        // Assert
        Assert.That(cell.Value, Is.EqualTo(value));
        Assert.That(cell.IsEmpty, Is.False);
    }

    [Test]
    public void SetValue_OnNonEmptyCell_UpdatesValue()
    {
        // Arrange
        var cell = new Cell(3);

        // Act
        cell.SetValue(7);

        // Assert
        Assert.That(cell.Value, Is.EqualTo(7));
    }

    [TestCase(0)]
    [TestCase(10)]
    [TestCase(-1)]
    public void SetValue_InvalidValue_ThrowsArgumentException(int invalidValue)
    {
        // Arrange
        var cell = new Cell();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => cell.SetValue(invalidValue));
        Assert.That(ex.Message, Does.Contain("Value must be between 1 and 9"));
    }

    [Test]
    public void Clear_NonPreFilledCell_ClearsValue()
    {
        // Arrange
        var cell = new Cell(5);

        // Act
        cell.Clear();

        // Assert
        Assert.That(cell.Value, Is.Null);
        Assert.That(cell.IsEmpty, Is.True);
    }

    [Test]
    public void Clear_EmptyCell_RemainsEmpty()
    {
        // Arrange
        var cell = new Cell();

        // Act
        cell.Clear();

        // Assert
        Assert.That(cell.Value, Is.Null);
        Assert.That(cell.IsEmpty, Is.True);
    }

    [Test]
    public void SetValue_PreFilledCell_ThrowsInvalidOperationException()
    {
        // Arrange
        var cell = new Cell(5, isPreFilled: true);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => cell.SetValue(9));
        Assert.That(ex.Message, Does.Contain("pre-filled"));
    }

    [Test]
    public void Clear_PreFilledCell_ThrowsInvalidOperationException()
    {
        // Arrange
        var cell = new Cell(5, isPreFilled: true);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => cell.Clear());
        Assert.That(ex.Message, Does.Contain("pre-filled"));
    }

}
