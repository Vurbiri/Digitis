using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BlocksArea : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new(10, 20);
    [SerializeField] private Transform _container;

    private Block[,] _blocks;
    private readonly List<Block> _blocksAdd = new();
    
    private Block this[Vector2Int index]
    {
        get => _blocks[index.x, index.y];
        set => _blocks[index.x, index.y] = value;
    }

    public event Action<int> EventAddPoints;
    public Transform Container => _container;
    public Vector2Int Size => _size;

    private const int ADD_VIRTUAL_Y_SIZE = 5;

    private void Awake()
    {
        _blocks = new Block[_size.x, _size.y + ADD_VIRTUAL_Y_SIZE];
    }

    #region IsEmpty...
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
    public bool IsEmptyCell(Vector2Int index) => IsCorrectIndex(index) && this[index] == null;
    public bool IsEmptyDownstairs(Block block) => IsEmptyDownstairs(block.Position);
    public bool IsEmptyDownstairs(Vector2Int index) => IsCorrectIndexY(--index.y) && this[index] == null;
    #endregion

    public async UniTask<Dictionary<int, Vector2Int>> CheckSeriesBlocksAsync()
    {
        int count = _blocksAdd.Count;
        if (count == 0) return null;

        HashSet <Block> blocksSeries = new();
        HashSet<Block> blocksOne = new();
        HashSet <Block> blocksRemove = new();
        
        Block tempBlock;
        int Points = 0;
        int digit;

        _blocksAdd.Sort();
        for(int i = count - 1; i >= 0; i--)
        {
            tempBlock = _blocksAdd[i];
            if (this[tempBlock.Position] == null || (digit = tempBlock.Digit) == 1)
                continue;

            if (CreateSeries(tempBlock))
            {
                Points += digit * (2 * blocksSeries.Count + blocksOne.Count - digit);
                blocksRemove.UnionWith(blocksSeries);
                blocksRemove.UnionWith(blocksOne);
            }
            blocksSeries.Clear();
            blocksOne.Clear();
        }
        _blocksAdd.Clear();

        count = blocksRemove.Count;
        if (count == 0) return null;

        Dictionary<int, Vector2Int> columns = new(_size.x);
        List<UniTask> tasks = new(count);
        Vector2Int index;
        string s = "";
        foreach (var block in blocksRemove)
        {
            index = block.Position;
            if (columns.TryGetValue(index.x, out Vector2Int value))
            {
                value.x++;
                value.y = Mathf.Min(value.y, index.y);
            }
            else
            {
                value.x = 1;
                value.y = index.y;
            }
            columns[index.x] = value;

            s += block.Digit + " - " + index + " | ";

            tasks.Add(block.Remove());
        }
        Debug.Log(s);
        await UniTask.WhenAll(tasks);
        EventAddPoints?.Invoke(Points);

        return columns;

        #region Local Functions
        bool CreateSeries(Block block)
        {
            int dgt = block.Digit;

            if (dgt == 0) return false;
            if(!Add()) return false;

            foreach (var d in Direction2D.All)
                if(TryGetBlock(block.Position + d, out Block outBlock))
                    if(outBlock.IsOne || block.IsEqual(outBlock))
                        CreateSeries(outBlock);
            
            return blocksSeries.Count >= dgt;

            #region Local Functions
            bool Add()
            {
                if (dgt > 1)
                {
                    if (!blocksSeries.Add(block))
                        return false;
                }
                else
                {
                    if (!blocksOne.Add(block))
                        return false;
                }
                return true;
            }
            #endregion
        }
        bool TryGetBlock(Vector2Int index, out Block block)
        {
            block = null;
            if (!IsCorrectIndex(index))
                return false;

            block = this[index];
            return block != null;
        }
        #endregion
    }

    public List<Block> GetBlocksInColumn(int x, int minY)
    {
        List<Block> blocks = new();

        for (int y = minY; y <= _size.y; y++)
        {
            if (_blocks[x, y] != null)
            {
                blocks.Add(_blocks[x, y]);
                _blocks[x, y] = null;
            }
        }

        return blocks;
    }

    public void Add(Block block)
    {
        this[block.Position] = block;
        block.EventDeactivate += OnDeactivate;
        _blocksAdd.Add(block);
    }
    private void OnDeactivate(Block block)
    {
        block.EventDeactivate -= OnDeactivate;
        this[block.Position] = null;
    }

    private bool IsCorrectIndex(Vector2Int index) => IsCorrectIndexX(index.x)  && IsCorrectIndexY(index.y);
    private bool IsCorrectIndexX(int x) => x >= 0 && x < _size.x;
    private bool IsCorrectIndexY(int y) => y >= 0 && y < (_size.y + ADD_VIRTUAL_Y_SIZE);
}
