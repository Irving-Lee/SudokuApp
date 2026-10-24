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
        // Check rows first
        var rowViolation = ValidateRows(grid);
        if (rowViolation != null)
        {
            return rowViolation;
        }

        // Check columns second
        var columnViolation = ValidateColumns(grid);
        if (columnViolation != null)
        {
            return columnViolation;
        }

        // Check subgrids third
        var subgridViolation = ValidateSubgrids(grid);
        if (subgridViolation != null)
        {
            return subgridViolation;
        }

        // If no violations, return success
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

                // Skip empty cells
                if (cell.IsEmpty)
                {
                    continue;
                }

                int value = cell.Value!.Value;

                // Check for duplicate
                if (seen.Contains(value))
                {
                    char rowLetter = (char)('A' + row);
                    return ValidationResult.Failure($"Number {value} already exists in Row {rowLetter}.");
                }

                seen.Add(value);
            }
        }

        return null; // No violations found
    }

    private ValidationResult? ValidateColumns(Grid grid)
    {
        for (int col = 0; col < 9; col++)
        {
            var seen = new HashSet<int>();

            for (int row = 0; row < 9; row++)
            {
                var cell = grid.GetCell(new Coordinate(row, col));

                // Skip empty cells
                if (cell.IsEmpty)
                {
                    continue;
                }

                int value = cell.Value!.Value;

                // Check for duplicate
                if (seen.Contains(value))
                {
                    int columnNumber = col + 1; // Convert 0-based to 1-based
                    return ValidationResult.Failure($"Number {value} already exists in Column {columnNumber}.");
                }

                seen.Add(value);
            }
        }

        return null; // No violations found
    }

    private ValidationResult? ValidateSubgrids(Grid grid)
    {
        // There are 9 subgrids (3x3 arrangement)
        for (int subgridRow = 0; subgridRow < 3; subgridRow++)
        {
            for (int subgridCol = 0; subgridCol < 3; subgridCol++)
            {
                var seen = new HashSet<int>();

                // Check all 9 cells in this subgrid
                for (int cellRow = 0; cellRow < 3; cellRow++)
                {
                    for (int cellCol = 0; cellCol < 3; cellCol++)
                    {
                        int row = subgridRow * 3 + cellRow;
                        int col = subgridCol * 3 + cellCol;

                        var cell = grid.GetCell(new Coordinate(row, col));

                        // Skip empty cells
                        if (cell.IsEmpty)
                        {
                            continue;
                        }

                        int value = cell.Value!.Value;

                        // Check for duplicate
                        if (seen.Contains(value))
                        {
                            return ValidationResult.Failure($"Number {value} already exists in the same 3×3 subgrid.");
                        }

                        seen.Add(value);
                    }
                }
            }
        }

        return null; // No violations found
    }
}
