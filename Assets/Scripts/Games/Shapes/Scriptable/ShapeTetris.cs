using UnityEngine;

[CreateAssetMenu(fileName = "NewShapeTetris", menuName = "Digitis/Shape Tetris", order = 51)]
public class ShapeTetris : ScriptableObject
{
    [SerializeField] private Color _colorBlock = Color.white;
    [SerializeField] private Sprite _spriteBlock;

    public Color ColorBlock => _colorBlock;
    public Sprite SpriteBlock => _spriteBlock;
}
