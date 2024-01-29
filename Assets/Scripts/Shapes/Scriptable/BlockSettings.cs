using UnityEngine;

[CreateAssetMenu(fileName = "Block_", menuName = "Digitis/Block", order = 51)]
public class BlockSettings : ScriptableObject
{
    [SerializeField] private int _digit;
    [SerializeField] private Color _colorBlock = Color.white;
    [SerializeField] private Color _colorNumber = new(0.85f, 1f, 0.9f, 1f);
    [SerializeField] private Sprite _spriteNumber;

    public int Digit => _digit;
    public Color ColorBlock => _colorBlock;
    public Color ColorNumber => _colorNumber;
    public Sprite SpriteNumber => _spriteNumber;
}
