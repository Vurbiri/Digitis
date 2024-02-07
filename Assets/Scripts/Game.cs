using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Game : MonoBehaviour
{
    [SerializeField] private BlocksArea _area;
    [SerializeField] private ShapesManager _shapesManager;
    [SerializeField] private AGameController _gameController;
    [Header("Digitis")]
    [SerializeField] private int _startCountShapesForLevel_D = 20;
    [SerializeField] private int _countShapesPerLevel_D = 2;
    [Header("Tetris")]
    [SerializeField] private int _startCountShapesForLevel_T = 15;
    [SerializeField] private int _countShapesPerLevel_T = 2;
    [SerializeField] private int _pointsPerLine = 100;
    [Space]
    [SerializeField] private ShapeSize _shapeType = ShapeSize.Tromino;
    [SerializeField, Range(3,9)] private int _maxDigit = 9;
    [SerializeField] private bool _isGravity = true;
    [SerializeField] private bool _isTetris = false;

    private Dictionary<int, Vector2Int> _columns;
    private List<int> _lines;

    private int _currentShapes;
    private int _currentLevel = 1;

    public int Level { get => _currentLevel; private set { _currentLevel = value; EventChangeLevel?.Invoke(value.ToString()); } }
    public ScoreGame Score { get; private set; }

    public event Action<string> EventChangeLevel;

    private void Awake()
    {
        Score = new(0, _pointsPerLine);

        _area.ActionCalkScoreDigitis = Score.CalkScoreDigitis;
        _area.ActionCalkScoreTetris = Score.CalkScoreTetris;

        _gameController.EventLeftPress += _shapesManager.Left;
        _gameController.EventRightPress += _shapesManager.Right;
        _gameController.EventStartDown += _shapesManager.StartMoveDown;
        _gameController.EventEndDown += _shapesManager.EndMoveDown;
        _gameController.EventRotationPress += _shapesManager.Rotate;
        _gameController.EventBombClick += OnBomb;

    }

    private void Start()
    {
        Level = 10;

        if (_isTetris)
        {
            SetCurrentShapes(_startCountShapesForLevel_T, _countShapesPerLevel_T);
            _shapesManager.InitializeTetris(_shapeType);
            _shapesManager.EventEndMoveDown += OnBlockEndMoveDownTetris;
        }
        else
        {
            SetCurrentShapes(_startCountShapesForLevel_D, _countShapesPerLevel_D);
            _shapesManager.InitializeDigitis(_maxDigit, _shapeType);
            _shapesManager.EventEndMoveDown += OnBlockEndMoveDownDigitis;
        }


        _shapesManager.CreateShape();

        if (_isTetris)
            OnBlockEndMoveDownTetris();
        else
            OnBlockEndMoveDownDigitis();

        StartCoroutine(Rotate());
        StartCoroutine(Shift());

        //_gameController.ControlEnable = true;
    }

    private void OnBlockEndMoveDownDigitis()
    {
        if (FallColumns())
            return;

        OnBlockEndMoveDownAsync().Forget();

        #region Local Functions
        async UniTaskVoid OnBlockEndMoveDownAsync()
        {
            _columns = await _area.CheckNewBlocksDigitisAsync();
            if (FallColumns())
                return;

            if (_shapesManager.StartMove(_isGravity, Level))
            {
                if (--_currentShapes == 0)
                    LevelUp(_startCountShapesForLevel_D, _countShapesPerLevel_D);
            }
            else
            {
                _shapesManager.EventEndMoveDown -= OnBlockEndMoveDownDigitis;
                StopCoroutine(Rotate());
                StopCoroutine(Shift());
                Debug.Log("Stop");
            }
        }

        bool FallColumns()
        {
            if (_columns == null || _columns.Count == 0)
                return false;

            List<Block> blocks;
            KeyValuePair<int, Vector2Int> element;

            do
            {
                element = _columns.First(); 
                 _columns.Remove(element.Key);
                blocks = _area.GetBlocksInColumn(element.Key, element.Value.y);
            }
            while (blocks.Count == 0 && _columns.Count > 0);

            if(blocks.Count == 0)
                return false;

            _shapesManager.StartFall(blocks, _isGravity, element.Value.x);
            return true;
        }
        #endregion
    }
    private void OnBlockEndMoveDownTetris()
    {
        if (FallLines())
            return;

        OnBlockEndMoveDownAsync().Forget();

        #region Local Functions
        async UniTaskVoid OnBlockEndMoveDownAsync()
        {
            _lines = await _area.CheckNewBlocksTetrisAsync();
            if (FallLines())
                return;

            if (!_shapesManager.StartMove(false, Level))
            {
                if (--_currentShapes == 0)
                    LevelUp(_startCountShapesForLevel_T, _countShapesPerLevel_T);
            }
            else
            {
                _shapesManager.EventEndMoveDown -= OnBlockEndMoveDownTetris;
                StopCoroutine(Rotate());
                StopCoroutine(Shift());
                Debug.Log("Stop");
            }
        }

        bool FallLines()
        {
            if (_lines == null || _lines.Count == 0)
                return false;

            List<Block> blocks;
            int y;
            do
            {
                y = _lines[0];
                _lines.RemoveAt(0);
                blocks = _area.GetBlocksAboveLine(y);
            }
            while (blocks.Count == 0 && _lines.Count > 0);

            if (blocks.Count == 0)
                return false;

            _shapesManager.StartFall(blocks, false, 1);
            return true;
        }
        #endregion
    }

    private void LevelUp(int startShapes, int perLevel)
    {
        Level++;
        SetCurrentShapes(startShapes, perLevel);
    }

    private void SetCurrentShapes(int startShapes, int perLevel) => _currentShapes = startShapes + perLevel * (Level - 1);

    private void OnBomb()
    {
        if (_shapesManager.ShapeToBomb())
        {

        }
    }

    private IEnumerator Rotate()
    {
        while(true) 
        {
            yield return new WaitForSeconds(3f / Level);
            _shapesManager.Rotate();
        }
    }

    private IEnumerator Shift()
    {
        while (true)
        {
            if (UnityEngine.Random.value > 0.5) _shapesManager.Left(); else _shapesManager.Right();
            yield return new WaitForSeconds(1.3f / Level);
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _shapesManager.ShapeToBomb();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _shapesManager.Left();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _shapesManager.Right();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _shapesManager.Rotate();
        }

    }


    private void OnDestroy()
    {
        _gameController.EventLeftPress -= _shapesManager.Left;
        _gameController.EventRightPress -= _shapesManager.Right;
        _gameController.EventStartDown -= _shapesManager.StartMoveDown;
        _gameController.EventEndDown -= _shapesManager.EndMoveDown;
        _gameController.EventRotationPress -= _shapesManager.Rotate;
        _gameController.EventBombClick -= OnBomb;
    }
}
