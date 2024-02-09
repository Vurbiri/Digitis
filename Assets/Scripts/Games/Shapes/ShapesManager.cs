using Cysharp.Threading.Tasks;
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
    private GameData _gameData;
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

        _gameData = GameData.InstanceF;
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

    public bool ShapeToBomb() => _funcShapeToBomb.Invoke();

    public bool StartMove(bool isGravity, int level)
    {
        _speeds.Level = level;

        if (_shapeControl.SetupForNew(_shape, isGravity))
        {
            _actionCreateShape.Invoke();
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

    public void Left() => _shapeControl.TryShift(Vector2Int.left);
    public void Right() => _shapeControl.TryShift(Vector2Int.right);

    public void StartMoveDown() => _shapeControl.SetSpeed(true);
    public void EndMoveDown() => _shapeControl.SetSpeed(false);

    public void Rotate() => _shapeControl.TryRotate();

    public List<Block> GetBlocksInColumn(int x, int minY) => _area.GetBlocksInColumn(x, minY);
    public List<Block> GetBlocksAboveLine(int y) => _area.GetBlocksAboveLine(y);

    #region Digitis
    public void InitializeDigitis()
    {
        Shape[] shapes = Initialize(_gameData.MaxDigit, _gameData.ShapeType);
        if (_gameData.ModeStart == GameModeStart.GameNew)
        {
            CreateShapeDigitis();
        }
        else
        {
            CreateShapeDigitisContinue(_gameData.NextShape, _gameData.NextBlocksShape);
            _area.SetArea(GetBlockDigitis);
        }

        #region Local Functions
        Shape[] Initialize(int maxDigit, ShapeSize shape)
        {
            _actionCreateShape = CreateShapeDigitis;
            _funcShapeToBomb = () => _shape.ToBomb(_settingBomb);
            _settingsBlocks[0].Weight = 100 + BASE_WEIGHT_ONE * maxDigit;
            _settingsBlocks[0].MaxCount = shape.ToInt() - 1;
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
        void CreateShapeDigitisContinue(ShapeType nextShape, int[] nextBlocksShape)
        {
            _shape = shapes[nextShape.ToInt()];

            if (nextBlocksShape[0] == 0)
            {
                _shape.CreateBomb(_poolBlocks.GetObjects(_nextContainer, _countBlocks), _settingBomb);
                return;
            }

            BlockSettings[] settings = new BlockSettings[nextBlocksShape.Length];
            for (int i = 0; i < nextBlocksShape.Length; i++)
                settings[i] = _settingsBlocks[nextBlocksShape[i] - 1];

            _shape.CreateDigitis(_poolBlocks.GetObjects(_nextContainer, _countBlocks), settings);
        }
        Block GetBlockDigitis(Vector2 position, int digit)
        {
            Block block = _poolBlocks.GetObject(_area.Container);
            if (digit > 0)
                block.SetupDigitis(position, _settingsBlocks[digit - 1]);
            else
                block.SetupDigitis(position, _settingBomb);
            return block;
        }
        #endregion
    }

    public void SaveShapeDigitis()
    {
        SaveShapeTetris();

        int count = _shape.Blocks.Count;
        if (_gameData.NextBlocksShape.Length != count)
            _gameData.NextBlocksShape = new int[count];
        for (int i = 0; count > i; i++)
            _gameData.NextBlocksShape[i] = _shape.Blocks[i].Digit;

    }

    private void CreateShapeDigitis()
    {
        _shape = _randomShapes.Next;
        _shape.CreateDigitis(_poolBlocks.GetObjects(_nextContainer, _countBlocks), _randomBlockSettings.NextRange(_countBlocks));
    }

    public UniTask<Dictionary<int, Vector2Int>> CheckNewBlocksDigitisAsync() => _area.CheckNewBlocksDigitisAsync();
    #endregion

    #region Tetris
    public void InitializeTetris()
    {
        Initialize();
        if (_gameData.ModeStart == GameModeStart.GameNew)
        {
            CreateShapeTetris();
        }
        else
        {
            CreateShapeTetrisContinue(_gameData.NextShape);
            _area.SetArea(GetBlockTetris);
        }

        #region Local Functions
        void Initialize()
        {
            _funcShapeToBomb = () => false;
            _actionCreateShape = CreateShapeTetris;
            _randomShapes = new(_tetromino);
            _countBlocks = 4;
        }
        void CreateShapeTetrisContinue(ShapeType nextShape)
        {
            _shape = _tetromino[nextShape.ToInt()];
            _shape.CreateTetris(_poolBlocks.GetObjects(_nextContainer, _countBlocks), _particleMaterialTetris);
        }
        Block GetBlockTetris(Vector2 position, int id)
        {
            Block block = _poolBlocks.GetObject(_area.Container);
            block.SetupTetris(position, _tetromino[id].ShapeTetris, id, _particleMaterialTetris);
            return block;
        }
        #endregion
    }

    public void SaveShapeTetris()
    {
        _area.SaveArea();
        _gameData.NextShape = _shape.Type;
    }

    private void CreateShapeTetris()
    {
        _shape = _randomShapes.Next;
        _shape.CreateTetris(_poolBlocks.GetObjects(_nextContainer, _countBlocks), _particleMaterialTetris);
    }

    public UniTask<List<int>> CheckNewBlocksTetrisAsync() => _area.CheckNewBlocksTetrisAsync();
    #endregion
}
