using UnityEngine;

[CreateAssetMenu(fileName = "Block_", menuName = "Digitis/Block", order = 51)]
public class BlockSettings : ScriptableObject, IRandomizeObject
{
    [SerializeField] private int _digit;
    [SerializeField] private Color _colorBlock = Color.white;
    [SerializeField] private Sprite _spriteBlock;
    [SerializeField] private Color _colorNumber = Color.white;
    [SerializeField] private Sprite _spriteNumber;
    [SerializeField] private Material _materialParticle;
    [Header("Random")]
    [SerializeField] private int _randomWeight = 1;
    [SerializeField] private int _maxCount = 1;

    public int Digit => _digit;
    public Color ColorBlock => _colorBlock;
    public Sprite SpriteBlock => _spriteBlock;
    public Color ColorNumber => _colorNumber;
    public Sprite SpriteNumber => _spriteNumber;
    public Material MaterialParticle => _materialParticle;
    public int Weight { get => _randomWeight; set => _randomWeight = value; }
    public int MaxCount  { get =>_maxCount; set => _maxCount = value; }
}
