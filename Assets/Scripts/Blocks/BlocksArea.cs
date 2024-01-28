using System;
using System.Collections.Generic;
using UnityEngine;

public class BlocksArea : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new(10, 20);
    [SerializeField] private Transform _container;
    private Block[,] _blocks;

    public Transform Container => _container;
    public Vector2Int Size => _size;
    public Block this[Vector2Int index]
    {
        get => _blocks[index.x, index.y];
        set => _blocks[index.x, index.y] = value;
    }

    private const int ADD_VIRTUAL_Y_SIZE = 5;

    private void Awake()
    {
        _blocks = new Block[_size.x, _size.y + ADD_VIRTUAL_Y_SIZE];
    }

    public bool IsEmptyArea(HashSet<Vector2Int> set, Vector2Int offset)
    {
        foreach(var s in set)
        if(!IsEmptyCell(s + offset))
                return false;
        return true;
    }

    public bool IsEmptyArea(List<Block> blocks, Vector2Int offset)
    {
        foreach (var block in blocks)
            if (!IsEmptyCell(block.Position + offset))
                return false;
        return true;
    }

    public bool IsEmptyCell(Vector2Int index)
    {
        return index.x >= 0 && index.x < _size.x && CheckingVertically(index);
    }

    public bool IsEmptyDownstairs(Block block)
    {
        return IsEmptyDownstairs(block.Position);
    }

    public bool IsEmptyDownstairs(Vector2Int index)
    {
        index.y -= 1;
        return CheckingVertically(index);
    }

    public void Add(Block block)
    {
        this[block.Position] = block;
        block.EventDeactivate += Remove;
    }

    private void Remove(Block block)
    {
        block.EventDeactivate -= Remove;
        this[block.Position] = null;
    }

    private bool CheckingVertically(Vector2Int index) => index.y >= 0 && this[index] == null;
}
