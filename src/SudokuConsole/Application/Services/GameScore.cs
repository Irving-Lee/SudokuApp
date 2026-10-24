namespace SudokuConsole.Application.Services;

/// <summary>
/// Represents a completed game score entry.
/// </summary>
public sealed record GameScore
{
    public string PlayerName { get; init; } = "Player";

    public Difficulty Difficulty { get; init; }

    public TimeSpan SolveTime { get; init; }

    public int Mistakes { get; init; }

    public DateTime CompletedAt { get; init; }
}
