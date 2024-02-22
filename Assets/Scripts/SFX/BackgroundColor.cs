using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
    [SerializeField] private Game _game;
    [Space]
    [SerializeField] private SpriteRenderer _tile;
    [SerializeField] private SpriteRenderer _back;
    [Space]
    [SerializeField] private Color _colorStart;
    [SerializeField] private Color _colorGameOver;
    [Space]
    [SerializeField] private float _brightnessBack = 0.9375f;
    [Space]
    [SerializeField] private Vector2 _rangeR = new(0.8f, 1.4f);
    [SerializeField] private Vector2 _rangeG = new(0.8f, 1.4f);
    [SerializeField] private Vector2 _rangeB = new(0.5f, 1.15f);

    private void Start()
    {
        SetColor(_colorStart);
        DataGame.Instance.EventChangeLevel += OnChangeLevel;
        _game.EventGameOver += () => SetColor(_colorGameOver);
    }

    private void OnChangeLevel(int level)
    {
        SetColor(_colorStart.RandomRecolor(_rangeR, _rangeG, _rangeB));
    }

    private void SetColor(Color color)
    {
        _tile.color = color;
        color.SetBrightness(_brightnessBack);
        _back.color = color;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            DataGame.Instance.EventChangeLevel -= OnChangeLevel;
    }
}
