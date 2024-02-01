using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BlocksArea : MonoBehaviour
{
    [SerializeField] private Vector2Int _size = new(10, 20);
    [SerializeField] private Transform _container;
    [Space]
    [SerializeField] private int _timePauseBlocksRemoved = 200;
    [SerializeField] private int _timePauseBombExploded = 150;

    private Block[,] _blocks;
    private readonly List<Block> _blocksAdd = new();
    private readonly List<Block> _bombsAdd = new();

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


    public async UniTask<Dictionary<int, Vector2Int>> CheckNewBlocksDigitisAsync()
    {
        if (_blocksAdd.Count > 0) 
            return await CheckSeriesBlocksAsync();
        else if (_bombsAdd.Count > 0)
            return await CheckBombsAsync();
        return null;
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
        if (block.IsBomb)
            _bombsAdd.Add(block);
        else
            _blocksAdd.Add(block);
    }

    private void OnDeactivate(Block block)
    {
        block.EventDeactivate -= OnDeactivate;
        this[block.Position] = null;
    }

    public async UniTask<Dictionary<int, Vector2Int>> CheckSeriesBlocksAsync()
    {
        HashSet<Block> blocksSeries = new();
        HashSet<Block> blocksOne = new();
        HashSet<Block> blocksRemove = new();

        Block tempBlock;
        int Points = 0;

        _blocksAdd.Sort();
        for (int i = _blocksAdd.Count - 1; i >= 0; i--)
        {
            tempBlock = _blocksAdd[i];
            if (this[tempBlock.Position] == null || tempBlock.IsOne)
                continue;

            if (CreateSeries(tempBlock))
            {
                Points += tempBlock.Digit * (2 * blocksSeries.Count + blocksOne.Count - tempBlock.Digit);
                blocksRemove.UnionWith(blocksSeries);
                blocksRemove.UnionWith(blocksOne);
            }
            blocksSeries.Clear();
            blocksOne.Clear();
        }
        _blocksAdd.Clear();

        if (blocksRemove.Count == 0) 
            return null;

        Dictionary<int, Vector2Int> columns = new(_size.x);
        List<UniTask> tasks = new(blocksRemove.Count);
        Vector2Int index;

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

            tasks.Add(block.Remove());
            await UniTask.Delay(_timePauseBlocksRemoved);
        }

        await UniTask.WhenAll(tasks);
        EventAddPoints?.Invoke(Points);

        return columns;

        #region Local Functions
        bool CreateSeries(Block block)
        {
            if (!AddToSeries()) 
                return false;

            foreach (var d in Direction2D.Cardinal)
                if (TryGetBlock(block.Position + d, out Block outBlock))
                    if (outBlock.IsOne || block.IsEqualDigit(outBlock))
                        CreateSeries(outBlock);

            return blocksSeries.Count >= block.Digit;

            #region Local Functions
            bool AddToSeries()
            {
                if (block.IsOne)
                {
                    if (!blocksOne.Add(block))
                        return false;
                }
                else
                {
                    if (!blocksSeries.Add(block))
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

    public async UniTask<Dictionary<int, Vector2Int>> CheckBombsAsync()
    {
        HashSet<Block> blocksExplode = new();
        //List<UniTask> tasks = new(blocksExplode.Count);

        foreach (var bomb in _bombsAdd)
        { 
            MarkBlocksToExplode(bomb);
            //tasks.Add(bomb.ExplodeBomb());
            //await UniTask.Delay(_timePauseBombExploded);
        }
        _bombsAdd.Clear();

        //if (blocksExplode.Count == 0)
        //{
        //    await UniTask.WhenAll(tasks);
        //    return null;
        //}

        Dictionary<int, Vector2Int> columns = new(_size.x);
        Vector2Int index;
        List<UniTask> tasks = new(blocksExplode.Count);

        foreach (var blockExplode in blocksExplode)
        {
            index = blockExplode.Position;
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

            tasks.Add(blockExplode.Explode());
            if(blockExplode.IsBomb)
                await UniTask.Delay(_timePauseBombExploded);
        }

        await UniTask.WhenAll(tasks);

        return columns;

        #region Local Functions
        void MarkBlocksToExplode(Block block)
        {
            blocksExplode.Add(block);
            foreach (var d in Direction2D.Rhomb)
                if (TryGetBlock(block.Position + d, out Block outBlock))
                    blocksExplode.Add(outBlock);
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

    private bool IsCorrectIndex(Vector2Int index) => IsCorrectIndexX(index.x)  && IsCorrectIndexY(index.y);
    private bool IsCorrectIndexX(int x) => x >= 0 && x < _size.x;
    private bool IsCorrectIndexY(int y) => y >= 0 && y < (_size.y + ADD_VIRTUAL_Y_SIZE);
}
