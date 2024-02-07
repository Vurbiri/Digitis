public class LevelBoard : ABoard
{
    private void Start()
    {
        SetValue(_game.Level.ToString());
        _game.EventChangeLevel += SetValue;

    }

    private void OnDestroy()
    {
        _game.EventChangeLevel -= SetValue;
    }
}
