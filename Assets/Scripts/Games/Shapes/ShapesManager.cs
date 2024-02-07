using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapesManager : MonoBehaviour
{
    #region SerializeField
    [Header("Blocks area")]
    [SerializeField] private BlocksArea _area;
    [Header ("Pool Block Data")]
    [SerializeField] private Block _prefabBlock;
    [SerializeField] private int _sizePool = 100;
    [SerializeField] private BlockSettings _settingBomb;
    [SerializeField] private BlockSettings _settingOne;
    [SerializeField] private BlockSettings[] _settingsBlocks;
    [Space]
    [SerializeField] private Material _particleMaterialTetris;
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
    private Pool<Block> _poolBlocks;

    private ShapeControl _shapeControl = null;
    private Shape _shapeForm = null;
    private Action _actionCreateShape;
    private Func<bool> _funcShapeToBomb;
    private RandomObjects<Shape> _randomShapes;
    private RandomObjects<BlockSettings> _randomBlockSettings;
    private int _countBlocks;


    private const int BASE_WEIGHT_ONE = 3;
    #endregion

    public event Action EventEndMoveDown;

    private void Awake()
    {
        Array.Sort(_settingsBlocks, (a,b) => a.Digit.CompareTo(b.Digit));

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

    public void CreateShape() => _actionCreateShape();

    public bool ShapeToBomb() => _funcShapeToBomb();

    public bool StartMove(bool isGravity, int level)
    {
        _speeds.Level = level;

        if (_shapeControl.SetupForNew(_shapeForm, isGravity))
        {
            _actionCreateShape();
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

    public void Left() => _shapeControl.Shift(Vector2Int.left);
    public void Right() => _shapeControl.Shift(Vector2Int.right);

    public void StartMoveDown() => _shapeControl.SetSpeed(true);
    public void EndMoveDown() => _shapeControl.SetSpeed(false);

    public void Rotate() => _shapeControl.Rotate();

    #region Digitis
    public void InitializeDigitis(int maxDigit, ShapeSize shape)
    {
        _actionCreateShape = CreateShapeDigitis;
        _funcShapeToBomb = () => _shapeForm.ToBomb(_settingBomb);
        _settingOne.Weight = 100 + BASE_WEIGHT_ONE * maxDigit;
        _settingOne.MaxCount = shape.ToInt() - 1;
        _randomBlockSettings = new(_settingsBlocks, maxDigit);
        _randomShapes = new( shape switch
        {
            ShapeSize.Domino => _domino,
            ShapeSize.Tromino => _tromino,
            ShapeSize.Tetromino => _tetromino,
            _ => null
        });
        _countBlocks = shape.ToInt();
    }
    private void CreateShapeDigitis()
    {
        _shapeForm = _randomShapes.Next;
        _shapeForm.CreateDigitis(_poolBlocks.GetObjects(_nextContainer, _countBlocks), _randomBlockSettings.NextRange(_countBlocks));
    }
    #endregion

    #region Tetris
    public void InitializeTetris(ShapeSize shape)
    {
        _funcShapeToBomb = () => false;
        _actionCreateShape = CreateShapeTetris;
        _randomShapes = new(shape switch
        {
            ShapeSize.Domino => _domino,
            ShapeSize.Tromino => _tromino,
            ShapeSize.Tetromino => _tetromino,
            _ => null
        });
        _countBlocks = shape.ToInt();
    }

    private void CreateShapeTetris()
    {
        _shapeForm = _randomShapes.Next;
        _shapeForm.CreateTetris(_poolBlocks.GetObjects(_nextContainer, _countBlocks), _particleMaterialTetris);
    }
    #endregion
}
