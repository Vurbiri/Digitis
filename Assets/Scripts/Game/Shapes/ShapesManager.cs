using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShapesManager : MonoBehaviour
{
    #region SerializeField
    [SerializeField] private ShapesManagerSFX _SFX;
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
    [Header("Shapes")]
    [SerializeField] private Shape[] _domino;
    [SerializeField] private Shape[] _tromino;
    [SerializeField] private Shape[] _tetromino;
    #endregion

    #region private
    private DataGame _gameData;
    private Pool<Block> _poolBlocks;
    private ShapeControl _shapeControl = null;
    private Shape _shape = null;
    private RandomObjects<Shape> _randomShapes;
    private RandomObjects<BlockSettings> _randomBlockSettings;
    private int _countBlocks;

    private const int BASE_WEIGHT_ONE = 3;
    #endregion

    public event Action EventEndMoveDown;

    private void Awake()
    {
        // ???
        Array.Sort(_settingsBlocks, (a, b) => a.Digit.CompareTo(b.Digit));
        Array.Sort(_tromino, (a, b) => a.Type.CompareTo(b.Type));
        Array.Sort(_tetromino, (a, b) => a.Type.CompareTo(b.Type));

        _gameData = DataGame.InstanceF;
        _poolBlocks = new(_prefabBlock, _poolRepository, _sizePool);
        _shapeControl = new ShapeControl(_area, _gameData.Speeds, _SFX.PlayFixed);
        _shapeControl.EventEndMoveDown += () => EventEndMoveDown?.Invoke();

        foreach (var d in _domino)
            d.Initialize();
        foreach (var t in _tromino)
            t.Initialize();
        foreach(var t in _tetromino)
            t.Initialize();
    }

    private void OnValidate()
    {
        Array.Sort(_settingsBlocks, (a, b) => a.Digit.CompareTo(b.Digit));
        Array.Sort(_tromino, (a, b) => a.Type.CompareTo(b.Type));
        Array.Sort(_tetromino, (a, b) => a.Type.CompareTo(b.Type));
    }

    public void Initialize()
    {
        Shape[] shapes = Initialize(_gameData.MaxDigit, _gameData.ShapeType);
        if (_gameData.ModeStart == GameModeStart.GameNew)
        {
            CreateShape();
        }
        else
        {
            CreateShapeContinue(_gameData.NextShape, _gameData.NextBlocksShape);
            _area.SetArea(GetBlock);
        }

        #region Local Functions
        Shape[] Initialize(int maxDigit, ShapeSize shape)
        {
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
        void CreateShapeContinue(ShapeType nextShape, int[] nextBlocksShape)
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

            _shape.CreateBlock(_poolBlocks.GetObjects(_nextContainer, _countBlocks), settings);
        }
        Block GetBlock(Vector2 position, int digit)
        {
            Block block = _poolBlocks.GetObject(_area.Container);
            if (digit > 0)
                block.Setup(position, _settingsBlocks[digit - 1]);
            else
                block.Setup(position, _settingBomb);
            return block;
        }
        #endregion
    }

    public bool ShapeToBomb()
    {
        bool result;
        if (result = _shape.ToBomb(_settingBomb))
            _SFX.PlayToBomb();
        return result;
    }

    public bool StartMove(int level)
    {
        _gameData.Speeds.Level = level;

        if (_shapeControl.SetupForNew(_shape))
        {
            CreateShape();
            _shapeControl.StartMoveDown();
            return true;
        }
        return false;
    }

    public void StartFall(List<Block> blocks)
    {
        _shapeControl.SetupForFall(blocks);
        _shapeControl.StartMoveDown();
    }

    public void Left()
    {
        if (_shapeControl.TryShift(Vector2Int.left))
            _SFX.PlayMove();
        else
            _SFX.PlayError();
    }
    public void Right()
    {
        if(_shapeControl.TryShift(Vector2Int.right))
            _SFX.PlayMove();
        else
            _SFX.PlayError();
    }

    public void StartMoveDown()
    {
        _shapeControl.SetSpeed(true);
        _SFX.PlayDown();
    }
    public void EndMoveDown() => _shapeControl.SetSpeed(false);

    public void Rotate()
    {
        if(_shapeControl.TryRotate())
            _SFX.PlayRotate();
        else
            _SFX.PlayError();
    }

    public List<Block> GetBlocksInColumn(int x, int minY) => _area.GetBlocksInColumn(x, minY);

    public void SaveShape()
    {
        _area.SaveArea();
        _gameData.NextShape = _shape.Type;

        int count = _shape.Blocks.Count;
        if (_gameData.NextBlocksShape.Length != count)
            _gameData.NextBlocksShape = new int[count];
        for (int i = 0; count > i; i++)
            _gameData.NextBlocksShape[i] = _shape.Blocks[i].Digit;

    }

    private void CreateShape()
    {
        _shape = _randomShapes.Next;
        _shape.CreateBlock(_poolBlocks.GetObjects(_nextContainer, _countBlocks), _randomBlockSettings.NextRange(_countBlocks));
    }

    public UniTask<Dictionary<int, int>> CheckNewBlocksAsync() => _area.CheckNewBlocksAsync();

    public UniTask RemoveAll() => _area.RemoveAll();
}
