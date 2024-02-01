using UnityEngine;

[CreateAssetMenu(fileName = "Block_", menuName = "Digitis/Block", order = 51)]
public class BlockSettings : ScriptableObject
{
    [SerializeField] private int _digit;
    [SerializeField] private Color _colorBlock = Color.white;
    [SerializeField] private Color _colorNumber = Color.white;
    [SerializeField] private Sprite _spriteNumber;
    [SerializeField] private Material _materialParticle;

    public int Digit => _digit;
    public Color ColorBlock => _colorBlock;
    public Color ColorNumber => _colorNumber;
    public Sprite SpriteNumber => _spriteNumber;
    public Material MaterialParticle => _materialParticle;
}
