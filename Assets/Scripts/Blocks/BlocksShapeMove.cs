using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlocksShapeMove
{
    public event Action EventEndMoveDown;

    private HashSet<Block> _blocks;

    private readonly Dictionary<int, BlockSettings> _blockSettings;

    private readonly Vector2Int[] positions;
    private readonly BlocksArea _area;

    private const int COUNT = 2;
    private const int MIN_DIGIT = 1;
    private const int MAX_DIGIT = 9;
    private const float SPEED_FALL = 20f;

    public BlocksShapeMove(BlockSettings[] settings, BlocksArea area)
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
        int moveCount = COUNT;
        Vector2Int position;
        int[] indexes = RandomRange();
        
        for (int i = 0; i < COUNT; i++)
        {
            position = positions[i];
            position.x += offset;
            Setup(blocks[i], position, _blockSettings[indexes[i]]);
            isEnd = isEnd || !_area.IsEmptyDownstairs(position);
        }

        if (isEnd)
            return false;

        _blocks = new(blocks);
        StartMove();

        return true;

        #region Local Functions
        void Setup(Block block, Vector2Int position, BlockSettings settings)
        {
            block.Setup(position, settings);
            block.EventEndMoveDown += OnEventEndMoveDown;
        }
        void StartMove()
        {
            foreach (var block in _blocks)
                block.MoveDown(speed);
        }
        void OnEventEndMoveDown(Block b)
        {
            if (!_area.IsEmptyDownstairs(b))
                FixedBlock(b);

            if (--moveCount > 0)
                return;

            CheckBlocks();

            moveCount = _blocks.Count;
            if (moveCount == 0)
            {
                EventEndMoveDown?.Invoke();
                return;
            }
            else if (moveCount < COUNT)
            {
                speed = SPEED_FALL;
            }

            StartMove();
        }
        void FixedBlock(Block b)
        {
            _area.Add(b);
            b.EventEndMoveDown -= OnEventEndMoveDown;
            _blocks.Remove(b);
        }
        void CheckBlocks()
        {
            Block bl = null;
            while (_blocks.Count > 0)
            {
                isEnd = true;
                foreach (var block in _blocks)
                {
                    if (!_area.IsEmptyDownstairs(block))
                    {
                        bl = block;
                        isEnd = false;
                        break;
                    }
                }
                if (!isEnd)
                    FixedBlock(bl);
                else
                    return;
            }
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
}
