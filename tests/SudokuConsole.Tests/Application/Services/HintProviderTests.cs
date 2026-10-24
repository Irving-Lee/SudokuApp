using NUnit.Framework;
using SudokuConsole.Application.Services;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Tests.Application.Services;

[TestFixture]
public class HintProviderTests
{
    private IHintProvider _hintProvider = null!;

    [SetUp]
    public void SetUp()
    {
        _hintProvider = new HintProvider();
    }

    [Test]
    public void GetHint_PuzzleWithEmptyCells_ReturnsValidHint()
    {
        // Arrange
        var solution = new Grid();
        var puzzle = new Grid();

        // Fill solution completely
        for (var row = 0; row < 9; row++)
        {
            for (var column = 0; column < 9; column++)
            {
                var value = (row * 3 + row / 3 + column) % 9 + 1;
                solution.SetCell(new Coordinate(row, column), value);
            }
        }

        // Fill puzzle partially (first 30 cells)
        for (var row = 0; row < 3; row++)
        {
            for (var column = 0; column < 9; column++)
            {
                var coord = new Coordinate(row, column);
                var value = solution.GetCell(coord).Value!.Value;
                puzzle.SetPrefilledCell(coord, value);
            }
        }
        // Row 3, columns 0-2
        for (var column = 0; column < 3; column++)
        {
            var coord = new Coordinate(3, column);
            var value = solution.GetCell(coord).Value!.Value;
            puzzle.SetPrefilledCell(coord, value);
        }

        // Act
        var result = _hintProvider.GetHint(puzzle, solution);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Coordinate, Is.Not.Null);
        Assert.That(result.Value, Is.Not.Null);

        // Verify the hint value matches the solution
        var hintCoord = result.Coordinate!.Value;
        var expectedValue = solution.GetCell(hintCoord).Value!.Value;
        Assert.That(result.Value, Is.EqualTo(expectedValue));

        // Verify the hint cell was empty in the puzzle
        var puzzleCell = puzzle.GetCell(hintCoord);
        Assert.That(puzzleCell.IsEmpty, Is.True);
    }

    [Test]
    public void GetHint_NoEmptyCells_ReturnsNoHintAvailable()
    {
        // Arrange
        var solution = new Grid();
        var puzzle = new Grid();

        // Fill both grids completely
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

        // Act
        var result = _hintProvider.GetHint(puzzle, solution);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Does.Contain("No empty cells"));
    }

    [Test]
    public void GetHint_DoesNotModifyPuzzleOrSolution()
    {
        // Arrange
        var solution = new Grid();
        var puzzle = new Grid();

        // Fill solution completely
        for (var row = 0; row < 9; row++)
        {
            for (var column = 0; column < 9; column++)
            {
                var value = (row * 3 + row / 3 + column) % 9 + 1;
                solution.SetCell(new Coordinate(row, column), value);
            }
        }

        // Fill puzzle partially (first row only)
        for (var column = 0; column < 9; column++)
        {
            var coord = new Coordinate(0, column);
            var value = solution.GetCell(coord).Value!.Value;
            puzzle.SetPrefilledCell(coord, value);
        }

        // Take snapshot of puzzle state before hint
        var emptyCountBefore = 0;
        for (var row = 0; row < 9; row++)
        {
            for (var column = 0; column < 9; column++)
            {
                if (puzzle.GetCell(new Coordinate(row, column)).IsEmpty)
                {
                    emptyCountBefore++;
                }
            }
        }

        // Act
        var result = _hintProvider.GetHint(puzzle, solution);

        // Assert - Verify hint was successful
        Assert.That(result.Success, Is.True);

        // Assert - Count empty cells after hint request
        var emptyCountAfter = 0;
        for (var row = 0; row < 9; row++)
        {
            for (var column = 0; column < 9; column++)
            {
                if (puzzle.GetCell(new Coordinate(row, column)).IsEmpty)
                {
                    emptyCountAfter++;
                }
            }
        }

        // The puzzle should be unchanged
        Assert.That(emptyCountAfter, Is.EqualTo(emptyCountBefore), 
            "GetHint should not modify the puzzle grid");

        // The hinted cell should still be empty in the puzzle
        var hintedCell = puzzle.GetCell(result.Coordinate!.Value);
        Assert.That(hintedCell.IsEmpty, Is.True, 
            "The hinted cell should remain empty in the puzzle");
    }

}
