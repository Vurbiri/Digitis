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
    private Shape _shape = null;
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

        Array.Sort(_tromino, (a, b) => a.ID.CompareTo(b.ID));
        Array.Sort(_tetromino, (a, b) => a.ID.CompareTo(b.ID));
    }

    public bool ShapeToBomb() => _funcShapeToBomb();

    public bool StartMove(bool isGravity, int level)
    {
        _speeds.Level = level;

        if (_shapeControl.SetupForNew(_shape, isGravity))
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
    public void InitializeNewDigitis(int maxDigit, ShapeSize shape)
    {
        InitializeDigitis(maxDigit, shape);
        CreateShapeDigitis();
    }
    public void InitializeContinueDigitis(int maxDigit, ShapeSize shape, ShapeType nextShape, int[] nextBlocksShape)
    {
        Shape[] shapes = InitializeDigitis(maxDigit, shape);
        CreateShape();

        #region Local Functions
        void CreateShape()
        {
            _shape = shapes[nextShape.ToInt()];

            if (nextBlocksShape[0] == 0)
            {
                _shape.CreateBomb(_poolBlocks.GetObjects(_nextContainer, _countBlocks), _settingBomb);
                return;
            }

            BlockSettings[] settings = new BlockSettings[nextBlocksShape.Length];
            for (int i = 0; i < nextBlocksShape.Length; i++)
                settings[i] = _settingsBlocks[nextBlocksShape[i]];

            _shape.CreateDigitis(_poolBlocks.GetObjects(_nextContainer, _countBlocks), settings);
        }
        #endregion
    }

    private Shape[] InitializeDigitis(int maxDigit, ShapeSize shape)
    {
        _actionCreateShape = CreateShapeDigitis;
        _funcShapeToBomb = () => _shape.ToBomb(_settingBomb);
        _settingOne.Weight = 100 + BASE_WEIGHT_ONE * maxDigit;
        _settingOne.MaxCount = shape.ToInt() - 1;
        _randomBlockSettings = new(_settingsBlocks, maxDigit);
        Shape[] shapes = shape switch
        {
            ShapeSize.Domino => _domino,
            ShapeSize.Tromino => _tromino,
            ShapeSize.Tetromino => _tetromino,
            _ => null
        };
        _randomShapes = new(shapes);
        _countBlocks = shape.ToInt();

        return shapes;
    }

    private void CreateShapeDigitis()
    {
        _shape = _randomShapes.Next;
        _shape.CreateDigitis(_poolBlocks.GetObjects(_nextContainer, _countBlocks), _randomBlockSettings.NextRange(_countBlocks));
    }
    #endregion

    #region Tetris
    public void InitializeNewTetris()
    {
        InitializeTetris();
        CreateShapeTetris();
    }
    public void InitializeContinueTetris(ShapeType nextShape)
    {
        InitializeTetris();
        CreateShape();

        #region Local Functions
        void CreateShape()
        {
            _shape = _tetromino[nextShape.ToInt()];
            _shape.CreateTetris(_poolBlocks.GetObjects(_nextContainer, _countBlocks), _particleMaterialTetris);
        }
        #endregion
    }
    private void InitializeTetris()
    {
        _funcShapeToBomb = () => false;
        _actionCreateShape = CreateShapeTetris;
        _randomShapes = new(_tetromino);
        _countBlocks = 4;
    }

    private void CreateShapeTetris()
    {
        _shape = _randomShapes.Next;
        _shape.CreateTetris(_poolBlocks.GetObjects(_nextContainer, _countBlocks), _particleMaterialTetris);
    }
    #endregion
}
