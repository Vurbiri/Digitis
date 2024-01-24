using System;
using UnityEngine;

public class BlocksArea
{
    public Block this[Vector2Int index]
    {
        get { return _blocks[index.x, index.y]; }
        set { _blocks[index.x, index.y] = value; }
    }

    public Vector2Int Size { get; }

    private readonly Block[,] _blocks;

    public BlocksArea(Vector2Int size)
    {
        _blocks = new Block[size.x, size.y];
        Size = size;
    }

    public bool IsEmptyDownstairs(Vector2Int index)
    {
        index.y -= 1;
        return index.y >= 0 && this[index] == null;
    }


    public void Add(Block block) => this[block.Index] = block;
}
