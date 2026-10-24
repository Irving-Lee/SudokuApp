namespace SudokuConsole.Domain.Models;

/// <summary>
/// Represents a 9x9 Sudoku grid.
/// </summary>
public class Grid
{
    private const int GridSize = 9;
    private readonly Cell[,] _cells;

    /// <summary>
    /// Gets a value indicating whether the grid is completely filled (no empty cells).
    /// </summary>
    public bool IsComplete
    {
        get
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    if (_cells[row, col].IsEmpty)
                    {
                        return false; // Early exit on first empty cell
                    }
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Grid"/> class with all cells empty.
    /// </summary>
    public Grid()
    {
        _cells = new Cell[GridSize, GridSize];
        
        // Initialize all cells as empty
        for (int row = 0; row < GridSize; row++)
        {
            for (int col = 0; col < GridSize; col++)
            {
                _cells[row, col] = new Cell();
            }
        }
    }

    /// <summary>
    /// Gets the cell at the specified coordinate.
    /// </summary>
    /// <param name="coordinate">The coordinate of the cell.</param>
    /// <returns>The cell at the specified coordinate.</returns>
    public Cell GetCell(Coordinate coordinate)
    {
        return _cells[coordinate.Row, coordinate.Column];
    }

    /// <summary>
    /// Sets the value of the cell at the specified coordinate.
    /// </summary>
    /// <param name="coordinate">The coordinate of the cell.</param>
    /// <param name="value">The value to set (1-9).</param>
    /// <exception cref="ArgumentException">Thrown when value is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown when trying to modify a pre-filled cell.</exception>
    public void SetCell(Coordinate coordinate, int value)
    {
        var cell = GetCell(coordinate);
        cell.SetValue(value);
    }

    /// <summary>
    /// Clears the value of the cell at the specified coordinate.
    /// </summary>
    /// <param name="coordinate">The coordinate of the cell to clear.</param>
    /// <exception cref="InvalidOperationException">Thrown when trying to clear a pre-filled cell.</exception>
    public void ClearCell(Coordinate coordinate)
    {
        var cell = GetCell(coordinate);
        cell.Clear();
    }

    /// <summary>
    /// Initializes a cell with a pre-filled value.
    /// Intended for constructing puzzles; player moves should use <see cref="SetCell"/>.
    /// </summary>
    /// <param name="coordinate">The cell coordinate.</param>
    /// <param name="value">The value to place, from 1 through 9.</param>
    public void SetPrefilledCell(Coordinate coordinate, int value)
    {
        _cells[coordinate.Row, coordinate.Column] = new Cell(value, isPreFilled: true);
    }
}
