using UnityEngine;

public class BlocksArea
{
    public Block this[Vector2Int index]
    {
        get => _blocks[index.x, index.y];
        set => _blocks[index.x, index.y] = value;
    }
    public Vector2Int Size { get; }

    private readonly Block[,] _blocks;

    public BlocksArea(Vector2Int size) 
    {
        _blocks = new Block[size.x, size.y];
        Size = size;
    }

    public bool IsEmptyCell(Vector2Int index)
    {
        return index.x >= 0 && index.x < Size.x && CheckingVertically(index);
    }

    public bool IsEmptyDownstairs(Block block)
    {
        return IsEmptyDownstairs(block.Index);
    }

    public bool IsEmptyDownstairs(Vector2Int index)
    {
        index.y -= 1;
        return CheckingVertically(index);
    }

    public void Add(Block block)
    {
        this[block.Index] = block;
        block.EventDeactivate += Remove;
    }

    private void Remove(Block block)
    {
        block.EventDeactivate -= Remove;
        this[block.Index] = null;
    }

    private bool CheckingVertically(Vector2Int index) => Size.y <= index.y || (index.y >= 0 && this[index] == null);
}
