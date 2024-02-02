using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewShape", menuName = "Digitis/Shape", order = 51)]
public class Shape : ScriptableObject
{
    [SerializeField] private int _countBlocks;
    [SerializeField] private int _sizeBound;
    [SerializeField] private Vector2 _offsetForNext;
    [SerializeField] private Vector2Int _offsetForArea;
    [SerializeField] private Vector2Int[] _startBlocksPositions;
    [Header("Tetris")]
    [SerializeField] private Color _colorBlock = Color.white;
    [SerializeField] private Sprite _spriteBlock;

    public int CountBlocks => _countBlocks;
    public Vector2Int StartOffset => _offsetForArea;
    public Vector2Int[] BlocksPositions => _startBlocksPositions;
    public SubShape SubShape { get; private set; }
    public List<Block> Blocks {get; private set;}

    private bool _isBomb = false;

    private const int COUNT_SUBSHAPE = 4;

    public void Initialize()
    {
        SubShape = new(_startBlocksPositions, _sizeBound - 1, COUNT_SUBSHAPE);
    }

    public void CreateDigitis(List<Block> blocks, BlockSettings[] settings)
    {
        _isBomb = false;
        Blocks = blocks;

        for (int i = 0; i < _countBlocks; i++)
            blocks[i].SetupDigitis(_startBlocksPositions[i] + _offsetForNext, settings[i]);
    }
    public void CreateTetris(List<Block> blocks, Material particleMaterial)
    {
        Blocks = blocks;

        for (int i = 0; i < _countBlocks; i++)
            blocks[i].SetupTetris(_startBlocksPositions[i] + _offsetForNext, _colorBlock, _spriteBlock, particleMaterial);
    }

    public bool ToBomb(BlockSettings settings)
    {
        if(_isBomb) 
            return false;

        _isBomb = true;
        foreach (var block in Blocks)
            block.TypeDigitisSetup(settings);

        return true;
    }

}
