public class ScoreBoard : ABoard
{
    private void Start()
    {
        SetValue(_game.Score.ToString());
        _game.EventChangePoints += SetValue;
    }
}
