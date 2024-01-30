using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapesManager : MonoBehaviour
{
    #region SerializeField
    [Header("Blocks area")]
    [SerializeField] private BlocksArea _area;
    [Header ("Pool Block Data")]
    [SerializeField] private Block _prefabBlock;
    [SerializeField] private int _sizePool = 100;
    [SerializeField] private BlockSettings _settingBomb;
    [SerializeField] private BlockSettings[] _settingsBlocks;
    [Space]
    [SerializeField] private Transform _poolRepository;
    [Space]
    [SerializeField] private Transform _nextContainer;
    [Header("Speeds")]
    [SerializeField] private Speeds _speeds;
    [Header("Shapes")]
    [SerializeField] private Shape[] _domino;
    [SerializeField] private Shape[] _tromino;
    [SerializeField] private Shape[] _tetromino;
    #endregion

    #region private
    private Dictionary<int, BlockSettings> _blockSettings;
    private Pool<Block> _poolBlocks;

    private Shape[] _currentShapes;
    private ShapeControl _shapeControl = null;
    private Shape _shapeForm = null;

    private int _minDigit = 1;
    private int _maxDigit;
    #endregion

    public event Action EventEndMoveDown;

    private void Awake()
    {
        _blockSettings = new(_settingsBlocks.Length);
        foreach (var block in _settingsBlocks)
            _blockSettings[block.Digit] = block;

        _poolBlocks = new(_prefabBlock, _poolRepository, _sizePool);

        _shapeControl = new ShapeControl(_area, _speeds);
        _shapeControl.EventEndMoveDown += () => EventEndMoveDown?.Invoke();

        foreach (var d in _domino)
            d.Initialize();
        foreach (var t in _tromino)
            t.Initialize();
        foreach(var t in _tetromino)
            t.Initialize();
    }

    public void Initialize(int maxDigit, ShapeSize shape)
    {
        _maxDigit = maxDigit;

        _currentShapes = shape switch
        {
            ShapeSize.Domino => _domino,
            ShapeSize.Tromino => _tromino,
            ShapeSize.Tetromino => _tetromino,
            _ => null
        };
    }

    public void CreateShape()
    {
        _shapeForm = _currentShapes[Random.Range(0, _currentShapes.Length)];
        int countBlocks = _shapeForm.CountBlocks;
        _shapeForm.Create(_poolBlocks.GetObjects(_nextContainer, countBlocks), RandomSettings());

        #region Local Functions
        BlockSettings[] RandomSettings()
        {
            int index;
            int[] indexes = new int[countBlocks];
            BlockSettings[] settings = new BlockSettings[countBlocks];
            BlockSettings setting;
            int digit;
            bool Equal = false;
            for (int i = 0; i < countBlocks; i++)
            {
                do
                {
                    index = Random.Range(_minDigit, _maxDigit + 1);
                    setting = _blockSettings[index];
                    digit = setting.Digit;
                    digit -= digit > 1 ? 1 : 0;

                    for (int j = 0; j < i; j++)
                    {
                        if (index == indexes[j])
                            digit--;
                        Equal = digit == 0;
                        if (Equal) break;
                    }

                } while (Equal);

                indexes[i] = index;
                settings[i] = setting;
            }
            return settings;
        }
        #endregion
    }

    public void ShapeToBomb() => _shapeForm.ReSetup(_settingBomb);

    public bool StartMove(bool isGravity)
    {
        if(_shapeControl.SetupForNew(_shapeForm, isGravity))
        {
            CreateShape();
            _shapeControl.StartMoveDown();
            return true;
        }
        return false;
    }

    public void StartFall(List<Block> blocks, bool isGravity, int count)
    {
        _shapeControl.SetupForFall(blocks, isGravity, count);
        _shapeControl.StartMoveDown();
    }

    public void Rotate() => _shapeControl.Rotate();

    public void Shift(Vector2Int direct)=> _shapeControl.Shift(direct);
}
