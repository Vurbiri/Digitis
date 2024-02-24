
public class ScoreBoard : ABoard
{
    private void Start()
    {
        SetText(_dataGame.Score.ToString());
        _dataGame.EventChangeScore += SetText;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeScore -= SetText;
    }
}
