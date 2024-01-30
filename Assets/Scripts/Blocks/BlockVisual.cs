using UnityEngine;

public class BlockVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRendererBlock;
    [SerializeField] private SpriteRenderer _spriteRendererNumber;
    [Header("Block")]
    [SerializeField] private Sprite _spriteBlock;
    [Header("Bomb")]
    [SerializeField] private Sprite _spriteBomb;


    public void Setup(BlockSettings settings)
    {
        if(settings.Digit == 0)
            _spriteRendererBlock.sprite = _spriteBomb;
        else
            _spriteRendererBlock.sprite = _spriteBlock;
        _spriteRendererBlock.color = settings.ColorBlock;

        _spriteRendererNumber.sprite = settings.SpriteNumber;
        _spriteRendererNumber.color = settings.ColorNumber;
        On();
    }

    public void Off()
    {
        _spriteRendererBlock.enabled = false;
        _spriteRendererNumber.enabled = false;
    }
    public void On()
    {
        _spriteRendererBlock.enabled = true;
        _spriteRendererNumber.enabled = true;
    }
}
