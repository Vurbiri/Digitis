using System;

public class ScoreGame 
{
    private int _value;
    private int _pointsPerLine;

    public int Value { get => _value;  private set { _value = value; EventChangePoints?.Invoke(value.ToString()); }  }

    public event Action<string> EventChangePoints;

    public ScoreGame(int pointsPerLine) : this(0, pointsPerLine) { }
    public ScoreGame(int score, int pointsPerLine)
    {
        _value = score;
        _pointsPerLine = pointsPerLine;
    }

    public void CalkScoreDigitis(int digit, int countSeries, int countOne)
    {
        Value += digit * (2 * countSeries + countOne - digit);
    }

    public void CalkScoreTetris(int countLine)
    {
        Value += _pointsPerLine * countLine;
    }
}
