public class ScoreBoard : ABoard
{
    private void Start()
    {
        SetValue(_game.Score.Value.ToString());
        _game.Score.EventChangePoints += SetValue;
    }
}
