public class Leaderboard
{ 
    public int UserRank { get; }
    public LeaderboardRecord[] Table { get; }

    public Leaderboard(int userRank, LeaderboardRecord[] table)
    {
        UserRank = userRank;
        Table = table;
    }
}

public class LeaderboardRecord
{
    public int Rank => _result.Rank;
    public int Score => _result.Score;
    public string Name { get; }
    public string AvatarURL { get; }

    private readonly LeaderboardResult _result;

    public LeaderboardRecord(int rank, int score, string name, string avatarURL)
    {
        _result = new(rank, score);
        Name = name;
        AvatarURL = avatarURL;
    }
}

public class LeaderboardResult
{
    public int Rank { get; }
    public int Score { get; }

    public LeaderboardResult(int rank, int score)
    {
        Rank = rank;
        Score = score;
    }
}
