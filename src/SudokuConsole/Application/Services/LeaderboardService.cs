
using System.Text.Json;

namespace SudokuConsole.Application.Services;

public class LeaderboardService : ILeaderboardService
{
    private const string FilePath = "leaderboard.json";

    public void SaveScore(GameScore score)
    {
        var scores = LoadAllScores();
        scores.Add(score);
        
        // Sort by time ascending, then mistakes ascending
        scores = scores
            .OrderBy(s => s.SolveTime)
            .ThenBy(s => s.Mistakes)
            .ToList();
        
        File.WriteAllText(FilePath, JsonSerializer.Serialize(scores));
    }

    public IReadOnlyList<GameScore> GetTopScores(Difficulty difficulty, int count = 10)
    {
        var scores = LoadAllScores();
        return scores
            .Where(s => s.Difficulty == difficulty)
            .OrderBy(s => s.SolveTime)
            .ThenBy(s => s.Mistakes)
            .Take(count)
            .ToList();
    }

    private List<GameScore> LoadAllScores()
    {
        if (!File.Exists(FilePath))
            return new List<GameScore>();

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<GameScore>>(json) 
               ?? new List<GameScore>();
    }
}
