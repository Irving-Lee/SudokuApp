namespace SudokuConsole.Application.Services;

/// <summary>
/// Provides persistence and retrieval for completed Sudoku scores.
/// </summary>
public interface ILeaderboardService
{
    void SaveScore(GameScore score);

    IReadOnlyList<GameScore> GetTopScores(Difficulty difficulty, int count = 10);
}
