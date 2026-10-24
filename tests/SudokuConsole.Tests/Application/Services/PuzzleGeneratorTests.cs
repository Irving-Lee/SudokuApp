using NUnit.Framework;
using SudokuConsole.Application.Services;
using SudokuConsole.Domain.Models;

namespace SudokuConsole.Tests.Application.Services;

[TestFixture]
public class PuzzleGeneratorTests
{
    [Test]
    public void Generate_ReturnsPuzzleWith30PrefilledCellsAndCompleteSolution()
    {
        // Arrange
        var generator = new PuzzleGenerator();

        // Act
        var generated = generator.Generate(Difficulty.Hard);

        // Assert
        Assert.That(generated.Puzzle, Is.Not.Null);
        Assert.That(generated.Solution, Is.Not.Null);

        Assert.That(CountPrefilledCells(generated.Puzzle), Is.EqualTo(30));
        Assert.That(CountFilledCells(generated.Solution), Is.EqualTo(81));
    }

    private static int CountPrefilledCells(Grid grid)
    {
        var count = 0;

        for (var row = 0; row < 9; row++)
        {
            for (var column = 0; column < 9; column++)
            {
                if (grid.GetCell(new Coordinate(row, column)).IsPreFilled)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private static int CountFilledCells(Grid grid)
    {
        var count = 0;

        for (var row = 0; row < 9; row++)
        {
            for (var column = 0; column < 9; column++)
            {
                if (!grid.GetCell(new Coordinate(row, column)).IsEmpty)
                {
                    count++;
                }
            }
        }

        return count;
    }

    [Test]
    public void Generate_PuzzleGivensMatchSolution_AndOtherCellsAreEmpty()
    {
        // Arrange
        var generator = new PuzzleGenerator();

        // Act
        var generated = generator.Generate(Difficulty.Hard);

        // Assert
        for (var row = 0; row < 9; row++)
        {
            for (var column = 0; column < 9; column++)
            {
                var coordinate = new Coordinate(row, column);
                var puzzleCell = generated.Puzzle.GetCell(coordinate);
                var solutionCell = generated.Solution.GetCell(coordinate);

                if (puzzleCell.IsPreFilled)
                {
                    Assert.That(puzzleCell.IsEmpty, Is.False);
                    Assert.That(puzzleCell.Value, Is.EqualTo(solutionCell.Value));
                }
                else
                {
                    Assert.That(puzzleCell.IsEmpty, Is.True);
                }
            }
        }
    }

    [Test]
    public void Generate_SolutionHasNoSudokuRuleViolations()
    {
        // Arrange
        var generator = new PuzzleGenerator();
        var validator = new SudokuValidator();

        // Act
        var generated = generator.Generate(Difficulty.Hard);
        var result = validator.Validate(generated.Solution);

        // Assert
        Assert.That(result.IsValid, Is.True, result.ViolationMessage);
    }

    [Test]
    public void Generate_PuzzleHasAtLeastOnePrefilledCellInEveryRowAndColumn()
    {
        // Arrange
        var generator = new PuzzleGenerator();

        // Act
        var generated = generator.Generate(Difficulty.Hard);

        // Assert
        for (var index = 0; index < 9; index++)
        {
            var rowHasClue = false;
            var columnHasClue = false;

            for (var otherIndex = 0; otherIndex < 9; otherIndex++)
            {
                rowHasClue |= generated.Puzzle
                    .GetCell(new Coordinate(index, otherIndex))
                    .IsPreFilled;

                columnHasClue |= generated.Puzzle
                    .GetCell(new Coordinate(otherIndex, index))
                    .IsPreFilled;
            }

            Assert.That(rowHasClue, Is.True, $"Row {(char)('A' + index)} should have a clue.");
            Assert.That(columnHasClue, Is.True, $"Column {index + 1} should have a clue.");
        }
    }

    [Test]
    public void Generate_MultipleCallsProduceDifferentSolutions()
    {
        // Arrange
        var generator = new PuzzleGenerator();

        // Act - Generate 5 puzzles
        var solutions = new List<Grid>();
        for (var i = 0; i < 5; i++)
        {
            solutions.Add(generator.Generate(Difficulty.Hard).Solution);
        }

        // Assert - At least one solution should differ from the first
        var firstSolution = solutions[0];
        var atLeastOneDifferent = false;

        for (var i = 1; i < solutions.Count; i++)
        {
            if (!GridsAreIdentical(firstSolution, solutions[i]))
            {
                atLeastOneDifferent = true;
                break;
            }
        }

        Assert.That(atLeastOneDifferent, Is.True, 
            "Multiple generations should produce at least some different solutions");
    }

    private static bool GridsAreIdentical(Grid grid1, Grid grid2)
    {
        for (var row = 0; row < 9; row++)
        {
            for (var column = 0; column < 9; column++)
            {
                var coord = new Coordinate(row, column);
                var value1 = grid1.GetCell(coord).Value;
                var value2 = grid2.GetCell(coord).Value;

                if (value1 != value2)
                {
                    return false;
                }
            }
        }

        return true;
    }

    [Test]
    public void Generate_EasyDifficulty_ReturnsPuzzleWith40PrefilledCells()
    {
        // Arrange
        var generator = new PuzzleGenerator();

        // Act
        var generated = generator.Generate(Difficulty.Easy);

        // Assert
        Assert.That(CountPrefilledCells(generated.Puzzle), Is.EqualTo(40));
        Assert.That(CountFilledCells(generated.Solution), Is.EqualTo(81));
    }

    [Test]
    public void Generate_MediumDifficulty_ReturnsPuzzleWith35PrefilledCells()
    {
        // Arrange
        var generator = new PuzzleGenerator();

        // Act
        var generated = generator.Generate(Difficulty.Medium);

        // Assert
        Assert.That(CountPrefilledCells(generated.Puzzle), Is.EqualTo(35));
        Assert.That(CountFilledCells(generated.Solution), Is.EqualTo(81));
    }

    [Test]
    public void Generate_HardDifficulty_ReturnsPuzzleWith30PrefilledCells()
    {
        // Arrange
        var generator = new PuzzleGenerator();

        // Act
        var generated = generator.Generate(Difficulty.Hard);

        // Assert
        Assert.That(CountPrefilledCells(generated.Puzzle), Is.EqualTo(30));
        Assert.That(CountFilledCells(generated.Solution), Is.EqualTo(81));
    }


}
