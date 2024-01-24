using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlocksShape
{
    public event Action<Block, bool> EventEndMoveDown;

    private Block[] _blocks;
    private int _currentCount;

    private readonly Dictionary<int, BlockSettings> _blockSettings;

    private readonly Vector2Int[] positions;
    private readonly BlocksArea _area;

    private const int COUNT = 2;
    private const int MIN_DIGIT = 1;
    private const int MAX_DIGIT = 9;

    public BlocksShape(BlockSettings[] settings, BlocksArea area)
    {
        _blockSettings = new(settings.Length);
        foreach (var block in settings)
            _blockSettings[block.Digit] = block;

        int x = area.Size.x - 1;
        int y = area.Size.y - 1;
        positions = new Vector2Int[]
        {
            new(Mathf.FloorToInt(x / 2f), y),
            new(Mathf.CeilToInt(x / 2f), y)
        };
        _area = area;
    }

    public bool Spawn(Block[] blocks, float speed, int offset)
    {
        bool isEnd = false;
        Vector2Int position;
        int[] range = RandomRange();

        for (int i = 0; i < COUNT; i++)
        {
            position = positions[i];
            position.x += offset;
            Setup(blocks[i], position, _blockSettings[range[i]]);
            isEnd = isEnd || !_area.IsEmptyDownstairs(position);
        }

        if (isEnd)
            return false;

        foreach (var block in blocks)
            block.MoveDown(speed);

        _blocks = blocks;
        _currentCount = COUNT;
        return true;

        #region Local Functions
        void Setup(Block block, Vector2Int position, BlockSettings settings)
        {
            block.Setup(position, settings);
            block.EventEndMoveDown += OnEventEndMoveDown;
        }
        void OnEventEndMoveDown(Block b)
        {
            if (_area.IsEmptyDownstairs(b.Index))
            {
                b.MoveDown(speed);
                return;
            }

            b.EventEndMoveDown -= OnEventEndMoveDown;
            int index = Array.IndexOf(_blocks, b);
            _blocks[index] = null;
            isEnd = --_currentCount == 0;
            if (!isEnd)
                BlocksPause(true);

            EventEndMoveDown?.Invoke(b, isEnd);
        }
        int[] RandomRange()
        {
            int[] r = new int[COUNT];
            bool Equal = false;
            for(int i = 0; i < COUNT; i++)
            {
                do
                {
                    r[i] = Random.Range(MIN_DIGIT, MAX_DIGIT + 1);
                    for(int j = 0; j < i; j++)
                    {
                        Equal = r[i] == r[j];
                        if (Equal) break;
                    }

                }while (Equal);
            }
            return r;
        }
        #endregion
    }

    public void BlocksPause(bool pause)
    {
        foreach (var block in _blocks)
        {
            if(block != null)
                block.isMovePause = pause;
        }
    }
}
