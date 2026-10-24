using NUnit.Framework;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Tests.Domain;

[TestFixture]
public class CoordinateTests
{
    [Test]
    public void Constructor_ValidRowAndColumn_SetsProperties()
    {
        // Arrange & Act
        var coordinate = new Coordinate(0, 0);

        // Assert
        Assert.That(coordinate.Row, Is.EqualTo(0));
        Assert.That(coordinate.Column, Is.EqualTo(0));
    }

    [Test]
    public void Constructor_MiddleCoordinate_SetsProperties()
    {
        // Arrange & Act
        var coordinate = new Coordinate(4, 4);

        // Assert
        Assert.That(coordinate.Row, Is.EqualTo(4));
        Assert.That(coordinate.Column, Is.EqualTo(4));
    }

    [Test]
    public void Constructor_MaxCoordinate_SetsProperties()
    {
        // Arrange & Act
        var coordinate = new Coordinate(8, 8);

        // Assert
        Assert.That(coordinate.Row, Is.EqualTo(8));
        Assert.That(coordinate.Column, Is.EqualTo(8));
    }

    [TestCase(-1, 0)]
    [TestCase(-5, 4)]
    [TestCase(9, 0)]
    [TestCase(10, 5)]
    [TestCase(100, 0)]
    public void Constructor_InvalidRow_ThrowsArgumentException(int invalidRow, int validColumn)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Coordinate(invalidRow, validColumn));
        Assert.That(ex.Message, Does.Contain("Row"));
    }

    [TestCase(0, -1)]
    [TestCase(4, -5)]
    [TestCase(0, 9)]
    [TestCase(5, 10)]
    [TestCase(0, 100)]
    public void Constructor_InvalidColumn_ThrowsArgumentException(int validRow, int invalidColumn)
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => new Coordinate(validRow, invalidColumn));
        Assert.That(ex.Message, Does.Contain("Column"));
    }

    [TestCase("A1", 0, 0)]
    [TestCase("A9", 0, 8)]
    [TestCase("I1", 8, 0)]
    [TestCase("I9", 8, 8)]
    [TestCase("E5", 4, 4)]
    [TestCase("B3", 1, 2)]
    [TestCase("H7", 7, 6)]
    public void Parse_ValidCoordinate_ReturnsCorrectIndices(string input, int expectedRow, int expectedColumn)
    {
        // Act
        var coordinate = Coordinate.Parse(input);

        // Assert
        Assert.That(coordinate.Row, Is.EqualTo(expectedRow));
        Assert.That(coordinate.Column, Is.EqualTo(expectedColumn));
    }

    [TestCase("A0")]    // Column 0 (invalid)
    [TestCase("J1")]    // Row J (out of range)
    [TestCase("A10")]   // Column 10 (out of range)
    [TestCase("Z9")]    // Row Z (way out of range)
    [TestCase("")]      // Empty string
    [TestCase("   ")]   // Whitespace only
    [TestCase("AA")]    // Invalid format (two letters)
    [TestCase("11")]    // Invalid format (two digits)
    [TestCase("1A")]    // Reversed format
    [TestCase("A")]     // Missing column
    [TestCase("1")]     // Missing row
    [TestCase("ABC")]   // Too long
    public void Parse_InvalidCoordinate_ThrowsArgumentException(string input)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Coordinate.Parse(input));
    }

    [Test]
    public void Parse_NullInput_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Coordinate.Parse(null!));
    }

    [TestCase("a1", 0, 0)]
    [TestCase("A1", 0, 0)]
    [TestCase("i9", 8, 8)]
    [TestCase("I9", 8, 8)]
    [TestCase("E5", 4, 4)]
    [TestCase("e5", 4, 4)]
    [TestCase("b3", 1, 2)]
    [TestCase("B3", 1, 2)]
    public void Parse_CaseInsensitive_ReturnsCorrectCoordinate(string input, int expectedRow, int expectedColumn)
    {
        // Act
        var coordinate = Coordinate.Parse(input);

        // Assert
        Assert.That(coordinate.Row, Is.EqualTo(expectedRow));
        Assert.That(coordinate.Column, Is.EqualTo(expectedColumn));
    }


    [TestCase(0, 0, "A1")]
    [TestCase(0, 8, "A9")]
    [TestCase(8, 0, "I1")]
    [TestCase(8, 8, "I9")]
    [TestCase(4, 4, "E5")]
    [TestCase(1, 2, "B3")]
    [TestCase(7, 6, "H7")]
    public void ToString_ReturnsHumanReadableFormat(int row, int column, string expected)
    {
        // Arrange
        var coordinate = new Coordinate(row, column);

        // Act
        var result = coordinate.ToString();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

}
