using UnityEngine;

public class ShapeDomino : AShapeForm
{
    public override int Count => 2;
    public override Vector2Int Size => Vector2Int.one * 2;
    protected override Vector2Int[] BlocksPositions { get; }

    public ShapeDomino(Vector2Int sizeContainer) : base(sizeContainer)
    {
        BlocksPositions = new[]
        {
            Vector2Int.zero,
            Vector2Int.right
        };
    }
}
