using UnityEngine;

public class BlockVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRendererBlock;
    [SerializeField] private SpriteRenderer _spriteRendererNumber;
    [Header("Block")]
    [SerializeField] private Sprite _spriteBlock;
    [Header("Bomb")]
    [SerializeField] private Sprite _spriteBomb;

    public void SetupDigitisBlock(BlockSettings settings) => SetupDigitis(settings, _spriteBlock);
    public void SetupDigitisBomb(BlockSettings settings) => SetupDigitis(settings, _spriteBomb);
    private void SetupDigitis(BlockSettings settings, Sprite sprite)
    {
        _spriteRendererBlock.sprite = sprite;
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

}
