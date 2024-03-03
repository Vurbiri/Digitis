using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShape", menuName = "Digitis/Shape", order = 51)]
public class Shape : ScriptableObject, IRandomizeObject
{
    [SerializeField] private ShapeType _type;
    [SerializeField] private int _sizeBound;
    [SerializeField] private Vector2 _offsetForNext;
    [SerializeField] private Vector2Int _offsetForArea;
    [SerializeField] private Vector2Int _offsetForAreaInfinity;
    [SerializeField] private Vector2Int[] _startBlocksPositions;
    [Header("Random")]
    [SerializeField] private int _randomWeight = 1;

    public ShapeType Type => _type;
    public Vector2Int StartOffset => _offsetForArea;
    public Vector2Int StartOffsetInfinity => _offsetForAreaInfinity;
    public Vector2Int[] BlocksPositions => _startBlocksPositions;
    public SubShape SubShape { get; private set; }
    public List<Block> Blocks {get; private set;}
    public int Weight => _randomWeight;
    public int MaxCount => 1;

    private bool _isBomb = false;

    private const int COUNT_SUBSHAPE = 4;

    public void Initialize()
    {
        SubShape = new(_startBlocksPositions, _sizeBound - 1, COUNT_SUBSHAPE);
    }

    public void CreateBlock(List<Block> blocks, BlockSettings[] settings)
    {
        _isBomb = false;
        Blocks = blocks;

        for (int i = 0; i < Blocks.Count; i++)
            blocks[i].Setup(_startBlocksPositions[i] + _offsetForNext, settings[i]);
    }
    public void CreateBomb(List<Block> blocks, BlockSettings setting)
    {
        _isBomb = true;
        Blocks = blocks;

        for (int i = 0; i < Blocks.Count; i++)
            blocks[i].Setup(_startBlocksPositions[i] + _offsetForNext, setting);
    }

    public bool ToBomb(BlockSettings settings)
    {
        if(_isBomb) 
            return false;

        _isBomb = true;
        foreach (var block in Blocks)
            block.TypeSetup(settings);

        return true;
    }

}
