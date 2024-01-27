using System.Collections.Generic;
using UnityEngine;

public abstract class AShapeForm
{
    public Dictionary<Vector2Int, Block> Blocks => new(_blocks);
    public abstract int Count { get; }
    public abstract Vector2Int Size { get; }
    protected abstract Vector2Int[] BlocksPositions { get; }
    protected Vector2Int StartPosition { get; }
    
    protected readonly Dictionary<Vector2Int, Block> _blocks;

    public AShapeForm(Vector2Int sizeContainer)
    {
        _blocks = new(Count);
        StartPosition = (sizeContainer - Size) / 2;
    }

    public void Create(Block[] blocks, BlockSettings[] settings)
    {
        Vector2Int position;
        _blocks.Clear();

        for (int i = 0; i < Count; i++)
        {
            position = BlocksPositions[i] + StartPosition;
            blocks[i].Setup(position, settings[i]);
            _blocks[BlocksPositions[i]] = blocks[i];
        }
    }
}
