namespace SudokuConsole.Domain.Models;

/// <summary>
/// Represents an immutable coordinate in the Sudoku grid.
/// Row range: 0-8 (A-I), Column range: 0-8 (1-9).
/// </summary>
public readonly record struct Coordinate
{
    private const int MinIndex = 0;
    private const int MaxIndex = 8;

    /// <summary>
    /// Gets the zero-based row index (0-8, corresponding to A-I).
    /// </summary>
    public int Row { get; }

    /// <summary>
    /// Gets the zero-based column index (0-8, corresponding to 1-9).
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Coordinate"/> struct.
    /// </summary>
    /// <param name="row">The zero-based row index (0-8).</param>
    /// <param name="column">The zero-based column index (0-8).</param>
    /// <exception cref="ArgumentException">Thrown when row or column is out of valid range.</exception>
    public Coordinate(int row, int column)
    {
        ValidateIndex(row, nameof(row));
        ValidateIndex(column, nameof(column));

        Row = row;
        Column = column;
    }

    /// <summary>
    /// Parses a human-readable coordinate string (e.g., "A1", "I9") into a Coordinate.
    /// </summary>
    /// <param name="input">The coordinate string in format [A-I][1-9] (e.g., "A1", "E5").</param>
    /// <returns>A validated Coordinate instance.</returns>
    /// <exception cref="ArgumentException">Thrown when input format is invalid.</exception>
    public static Coordinate Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Coordinate string cannot be null or empty.", nameof(input));
        }

        input = input.Trim().ToUpperInvariant();

        if (input.Length != 2 ||
            input[0] < 'A' || input[0] > 'I' ||
            input[1] < '1' || input[1] > '9')
        {
            throw new ArgumentException(
                $"Invalid coordinate format: '{input}'. Expected format: [A-I][1-9] (e.g., A1, E5, I9).",
                nameof(input));
        }

        return new Coordinate(input[0] - 'A', input[1] - '1');
    }

    /// <summary>
    /// Returns the human-readable string representation of the coordinate (e.g., "A1", "E5").
    /// </summary>
    /// <returns>A string in format [A-I][1-9].</returns>
    public override string ToString()
    {
        char rowChar = (char)('A' + Row);
        char columnChar = (char)('1' + Column);
        return $"{rowChar}{columnChar}";
    }

    private static void ValidateIndex(int index, string parameterName)
    {
        if (index < MinIndex || index > MaxIndex)
        {
            string displayName = char.ToUpper(parameterName[0]) + parameterName.Substring(1);
            
            throw new ArgumentException(
                $"{displayName} must be between {MinIndex} and {MaxIndex}. Got: {index}",
                parameterName);
        }
    }
}
