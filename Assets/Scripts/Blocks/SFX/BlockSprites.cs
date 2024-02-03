#if UNITY_EDITOR
using NaughtyAttributes;
#endif
using UnityEngine;

public class BlockSprites : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRendererBlock;
    [SerializeField] private SpriteRenderer _spriteRendererNumber;
#if UNITY_EDITOR
    [SerializeField] private BlockSettings[] _settings;
    [SerializeField, Range(0,9)] private int _numSettings;
#endif

    public void SetupDigitis(BlockSettings settings)
    {
        _spriteRendererBlock.sprite = settings.SpriteBlock;
        _spriteRendererBlock.color = settings.ColorBlock;

        _spriteRendererNumber.sprite = settings.SpriteNumber;
        _spriteRendererNumber.color = settings.ColorNumber;

        _spriteRendererBlock.enabled = true;
        _spriteRendererNumber.enabled = true;
    }

    public void SetupTetris(Color color, Sprite sprite)
    {
        _spriteRendererBlock.sprite = sprite;
        _spriteRendererBlock.color = color;

        _spriteRendererBlock.enabled = true;
        _spriteRendererNumber.enabled = false;
    }

    public void Off()
    {
        _spriteRendererBlock.enabled = false;
        _spriteRendererNumber.enabled = false;
    }

#if UNITY_EDITOR
    [Button]
    public void SetupDigitis()
    {
        SetupDigitis(_settings[_numSettings]);
    }
#endif
}
