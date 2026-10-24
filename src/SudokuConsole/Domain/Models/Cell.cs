namespace SudokuConsole.Domain.Models;

/// <summary>
/// Represents a single cell in the Sudoku grid.
/// </summary>
public class Cell
{
    private const int MinValue = 1;
    private const int MaxValue = 9;

    /// <summary>
    /// Gets the value of the cell (1-9), or null if empty.
    /// </summary>
    public int? Value { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this cell is empty (has no value).
    /// </summary>
    public bool IsEmpty => !Value.HasValue;

    /// <summary>
    /// Gets a value indicating whether this cell was pre-filled and cannot be modified by the user.
    /// </summary>
    public bool IsPreFilled { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Cell"/> class as an empty, user-editable cell.
    /// </summary>
    public Cell()
    {
        Value = null;
        IsPreFilled = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Cell"/> class with a value.
    /// </summary>
    /// <param name="value">The cell value (1-9).</param>
    /// <param name="isPreFilled">Whether this cell is pre-filled and immutable.</param>
    /// <exception cref="ArgumentException">Thrown when value is not between 1 and 9.</exception>
    public Cell(int value, bool isPreFilled = false)
    {
        if (value < MinValue || value > MaxValue)
        {
            throw new ArgumentException(
                $"Value must be between {MinValue} and {MaxValue}. Got: {value}",
                nameof(value));
        }

        Value = value;
        IsPreFilled = isPreFilled;
    }

    /// <summary>
    /// Sets the value of the cell.
    /// </summary>
    /// <param name="value">The value to set (1-9).</param>
    /// <exception cref="ArgumentException">Thrown when value is not between 1 and 9.</exception>
    /// <exception cref="InvalidOperationException">Thrown when trying to modify a pre-filled cell.</exception>
    public void SetValue(int value)
    {
        if (IsPreFilled)
        {
            throw new InvalidOperationException("Cannot modify a pre-filled cell.");
        }

        if (value < MinValue || value > MaxValue)
        {
            throw new ArgumentException(
                $"Value must be between {MinValue} and {MaxValue}. Got: {value}",
                nameof(value));
        }

        Value = value;
    }

    /// <summary>
    /// Clears the value of the cell, making it empty.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when trying to clear a pre-filled cell.</exception>
    public void Clear()
    {
        if (IsPreFilled)
        {
            throw new InvalidOperationException("Cannot clear a pre-filled cell.");
        }

        Value = null;
    }
}
