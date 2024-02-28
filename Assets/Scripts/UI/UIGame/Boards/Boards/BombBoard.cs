public class BombBoard : ABoard
{
    private void Start()
    {
        SetValue(_dataGame.CountBombs);
        _dataGame.EventChangeCountBombs += SetValue;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeScore -= SetText;
    }
}
