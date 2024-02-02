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

    public async UniTask<List<int>> CheckNewBlocksTetrisAsync()
    {
        if (_blocksAdd.Count == 0)
            return null;

        List<int> listY = new();
        HashSet<int> setY = new();
        int y;
        bool isFullLine;

        _blocksAdd.Sort((a, b) => b.Position.y.CompareTo(a.Position.y));
        foreach (var block in _blocksAdd)
        {
            y = block.Position.y;
            if (!setY.Add(y))
                continue;

            isFullLine = true;
            for (int x = 0; x < Size.x; x++)
            {
                if (_blocks[x, y] == null)
                {
                    isFullLine = false;
                    break;
                }
            }

            if (isFullLine)
                listY.Add(y);
        }
        _blocksAdd.Clear();

        if (listY.Count == 0)
            return null;

        List<UniTask> tasks = new(Size.x);
        int count = Size.x - 1, halfCount = Size.x / 2 - 1;
        int Points = 0, countLine = 1;
        foreach (int posY in listY)
        {
            Points += 100 * countLine++;

            for (int x = halfCount; x >= 0; x--)
            {
                tasks.Add(_blocks[x, posY].Remove());
                tasks.Add(_blocks[count - x, posY].Remove());
                await UniTask.Delay(_timePauseBlocksRemoved);
            }
        }

        await UniTask.WhenAll(tasks);
        EventAddPoints?.Invoke(Points);

        return listY;
    }

    public List<Block> GetBlocksAboveLine(int y)
    {
        List<Block> blocks = new();

        for (int x = 0; x < _size.x; x++)
            blocks.AddRange(GetBlocksInColumn(x, y));

        return blocks;
    }

    public List<Block> GetBlocksInColumn(int x, int minY)
    {
        List<Block> blocks = new();

        for (int y = minY + 1; y <= _size.y; y++)
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

    private async UniTask<Dictionary<int, Vector2Int>> CheckSeriesBlocksAsync()
    {
        HashSet<Block> blocksSeries = new();
        HashSet<Block> blocksOne = new();
        HashSet<Block> blocksRemove = new();
        int Points = 0;

        _blocksAdd.Sort((a, b) => b.Digit.CompareTo(a.Digit));
        foreach (var block in _blocksAdd)
        {
            if (block.IsOne)
                continue;

            if (CreateSeries(block))
            {
                Points += block.Digit * (2 * blocksSeries.Count + blocksOne.Count - block.Digit);
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
            if (!AddToSeries(block)) 
                return false;

            foreach (var d in Direction2D.Cardinal)
                if (TryGetBlock(block.Position + d, out Block outBlock))
                    if (outBlock.IsOne || block.IsEqualDigit(outBlock))
                        CreateSeries(outBlock);

            return blocksSeries.Count >= block.Digit;

            #region Local Functions
            bool AddToSeries(Block addBlock)
            {
                if(blocksRemove.Contains(addBlock))
                    return false;

                if (addBlock.IsOne)
                {
                    if (!blocksOne.Add(addBlock))
                        return false;
                }
                else
                {
                    if (!blocksSeries.Add(addBlock))
                        return false;
                }
                return true;
            }
            #endregion
        }
        #endregion
    }
    private async UniTask<Dictionary<int, Vector2Int>> CheckBombsAsync()
    {
        HashSet<Block> blocksExplode = new();

        foreach (var bomb in _bombsAdd)
            MarkBlocksToExplode(bomb);

        _bombsAdd.Clear();

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
        #endregion
    }

    private bool TryGetBlock(Vector2Int index, out Block block)
    {
        block = null;
        if (!IsCorrectIndex(index))
            return false;

        block = this[index];
        return block != null;
    }

    private bool IsCorrectIndex(Vector2Int index) => IsCorrectIndexX(index.x)  && IsCorrectIndexY(index.y);
    private bool IsCorrectIndexX(int x) => x >= 0 && x < _size.x;
    private bool IsCorrectIndexY(int y) => y >= 0 && y < (_size.y + ADD_VIRTUAL_Y_SIZE);
}
