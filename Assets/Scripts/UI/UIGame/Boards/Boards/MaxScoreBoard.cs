
public class MaxScoreBoard : ABoard
{
    private void Start()
    {
        if (!DataGame.Instance.IsInfinityMode)
        {
            gameObject.SetActive(false);
            return;
        }

        SetText(_dataGame.MaxScore.ToString());
        _dataGame.EventChangeMaxScore += SetText;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null && DataGame.Instance.IsInfinityMode)
            _dataGame.EventChangeScore -= SetText;
    }
}
