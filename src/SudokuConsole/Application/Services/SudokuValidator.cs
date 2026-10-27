using SudokuConsole.Domain.Models;

namespace SudokuConsole.Application.Services;

/// <summary>
/// Validates Sudoku grids according to standard rules.
/// </summary>
public class SudokuValidator : ISudokuValidator
{
    /// <summary>
    /// Validates the entire grid for Sudoku rule violations.
    /// Priority: Row violations → Column violations → Subgrid violations.
    /// </summary>
    /// <param name="grid">The grid to validate.</param>
    /// <returns>A validation result indicating success or the first violation found.</returns>
    public ValidationResult Validate(Grid grid)
    {
        var rowViolation = ValidateRows(grid);
        if (rowViolation is not null)
        {
            return rowViolation;
        }

        var columnViolation = ValidateColumns(grid);
        if (columnViolation is not null)
        {
            return columnViolation;
        }

        var subgridViolation = ValidateSubgrids(grid);
        if (subgridViolation is not null)
        {
            return subgridViolation;
        }

        return ValidationResult.Success();
    }

    private ValidationResult? ValidateRows(Grid grid)
    {
        for (int row = 0; row < 9; row++)
        {
            var seen = new HashSet<int>();

            for (int col = 0; col < 9; col++)
            {
                var cell = grid.GetCell(new Coordinate(row, col));

                if (cell.Value is not int value)
                {
                    continue;
                }

                if (!seen.Add(value))
                {
                    char rowLetter = (char)('A' + row);
                    return ValidationResult.Failure($"Number {value} already exists in Row {rowLetter}.");
                }
            }
        }

        return null;
    }

    private ValidationResult? ValidateColumns(Grid grid)
    {
        for (int col = 0; col < 9; col++)
        {
            var seen = new HashSet<int>();

            for (int row = 0; row < 9; row++)
            {
                var cell = grid.GetCell(new Coordinate(row, col));

                if (cell.Value is not int value)
                {
                    continue;
                }

                if (!seen.Add(value))
                {
                    int columnNumber = col + 1;
                    return ValidationResult.Failure($"Number {value} already exists in Column {columnNumber}.");
                }
            }
        }

        return null;
    }

    private ValidationResult? ValidateSubgrids(Grid grid)
    {
        for (int subgridRow = 0; subgridRow < 3; subgridRow++)
        {
            for (int subgridCol = 0; subgridCol < 3; subgridCol++)
            {
                var seen = new HashSet<int>();

                for (int cellRow = 0; cellRow < 3; cellRow++)
                {
                    for (int cellCol = 0; cellCol < 3; cellCol++)
                    {
                        int row = subgridRow * 3 + cellRow;
                        int col = subgridCol * 3 + cellCol;

                        var cell = grid.GetCell(new Coordinate(row, col));

                        if (cell.Value is not int value)
                        {
                            continue;
                        }

                        if (!seen.Add(value))
                        {
                            return ValidationResult.Failure($"Number {value} already exists in the same 3×3 subgrid.");
                        }
                    }
                }
            }
        }

        return null;
    }
}
