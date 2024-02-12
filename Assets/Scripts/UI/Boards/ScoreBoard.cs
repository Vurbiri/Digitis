public class ScoreBoard : ABoard
{
    private void Start()
    {
        _dataGame = DataGame.InstanceF;

        SetValue(_dataGame.Score.ToString());
        _dataGame.EventChangeScore += SetValue;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeScore -= SetValue;
    }
}
