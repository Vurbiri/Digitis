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
        ReColor(_colorStart);
        DataGame.Instance.EventChangeLevel += OnChangeLevel;
        _game.EventGameOver += () => ReColor(_colorGameOver);
    }

    private void OnChangeLevel(int level)
    {
        Color color = _colorStart;
        color.r *= _rangeR.RandomRange();
        color.g *= _rangeG.RandomRange();
        color.b *= _rangeB.RandomRange();
        ReColor(color);
    }

    private void ReColor(Color color)
    {
        _tile.color = color;
        color *= _brightnessBack; color.a = 1f;
        _back.color = color;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            DataGame.Instance.EventChangeLevel -= OnChangeLevel;
    }
}
