using UnityEngine;

public class Domino : AShapes
{
    protected override int Count => 1;

    protected override AShapeForm[] Shapes { get; }

    public Domino(PoolBlockData poolBlockData, Speeds speeds, BlocksArea area, int maxDigit) : base(poolBlockData, speeds, area, maxDigit)
    {
        Shapes = new AShapeForm[] { new ShapeDomino(Vector2Int.one * 4) };
    }

    protected override AShapeForm RandomShape()
    {
        return Shapes[0];
    }
}
