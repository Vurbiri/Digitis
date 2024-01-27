using System;
using System.Collections.Generic;

public abstract class AShapes
{
    public event Action EventEndMoveDown;

    private readonly IReadOnlyDictionary<int, BlockSettings> _blockSettings;
    private readonly Pool<Block> _poolBlocks;

    protected ShapeControl _shapeControl = null;
    protected AShapeForm _shapeForm = null;

    protected abstract int Count { get; }
    protected abstract AShapeForm[] Shapes { get; }

    private readonly PoolBlockData _poolBlockData;

    private int MinDigit => PoolBlockData.MIN_DIGIT;
    private int MaxDigit { get; }


    public AShapes(PoolBlockData poolBlockData, Speeds speeds, BlocksArea area, int maxDigit)
    {
        _poolBlockData = poolBlockData;
        MaxDigit = maxDigit;

        Dictionary<int, BlockSettings> blockSettings = new (poolBlockData.BlockSettings.Length);
        foreach (var block in poolBlockData.BlockSettings)
            blockSettings[block.Digit] = block;
        _blockSettings = blockSettings;
        
        _poolBlocks = new(poolBlockData.PrefabBlock, poolBlockData.PoolRepository, _poolBlockData.SizePool);

        _shapeControl = new ShapeControl(area, speeds, poolBlockData.GameAreaContainer);
        _shapeControl.EventEndMoveDown += () => EventEndMoveDown?.Invoke();
    }

    public void CreateForm()
    {
        _shapeForm = RandomShape();
        int countBlocks = _shapeForm.Count;
        _shapeForm.Create(_poolBlocks.GetObjects(_poolBlockData.NextAreaContainer, countBlocks), RandomRange());

        #region Local Functions
        BlockSettings[] RandomRange()
        {
            int index;
            int[] r = new int[countBlocks];
            BlockSettings[] settings = new BlockSettings[countBlocks];
            bool Equal = false;
            for (int i = 0; i < countBlocks; i++)
            {
                do
                {
                    index = UnityEngine.Random.Range(MinDigit, MaxDigit + 1);
                    for (int j = 0; j < i; j++)
                    {
                        Equal = index == r[j];
                        if (Equal) break;
                    }

                } while (Equal);
                r[i] = index;
                settings[i] = _blockSettings[index];
            }
            return settings;
        }
        #endregion
    }

    public bool StartMove()
    {
        if(_shapeControl.Create(_shapeForm))
        {
            CreateForm();
            _shapeControl.StartMoveDown();
            return true;
        }
        return false;
    }

    public void Rotate() => _shapeControl.Rotate();

    protected abstract AShapeForm RandomShape();


}
