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
    [SerializeField] private int _countBombsStart = 3;
    [Header("Tetris")]
    [SerializeField] private int _startCountShapesForLevel_T = 15;
    [SerializeField] private int _countShapesPerLevel_T = 2;
    [SerializeField] private int _pointsPerLine = 50;

    private Dictionary<int, Vector2Int> _columns;
    private List<int> _lines;
    private GameData _gameData;
    private int _countShapesMax;

    private GameModeStart ModeStart { get => _gameData.ModeStart; set => _gameData.ModeStart = value; }
    private bool IsDigitis => _gameData.IsDigitis;

    public int Level { get => _gameData.CurrentLevel; private set { _gameData.CurrentLevel = value; EventChangeLevel?.Invoke(value.ToString()); } }
    public int CountBombs { get => _gameData.CountBombs; private set { _gameData.CountBombs = value; EventChangeCountBombs?.Invoke(value); } }
    public int CountShapes { get => _gameData.CountShapes; private set { _gameData.CountShapes = value; EventChangeCountShapes?.Invoke(value); } }
    public int CountShapesMax { get => _countShapesMax; private set { _countShapesMax = value; EventChangeCountShapesMax?.Invoke(value); } }

    public ScoreGame Score { get; private set; }

    public event Action<string> EventChangeLevel;
    public event Action<int> EventChangeCountBombs;
    public event Action<int> EventChangeCountShapes;
    public event Action<int> EventChangeCountShapesMax;

    private void Awake()
    {
        Score = new(0, _pointsPerLine);
        _gameData = GameData.InstanceF;

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
        if (IsDigitis)
            SetupDigitis();
        else
            SetupTetris();

        ModeStart = GameModeStart.GameContinue;
        _gameController.ControlEnable = true;

        if (IsDigitis)
            OnBlockEndMoveDownDigitis();
        else
            OnBlockEndMoveDownTetris();

        StartCoroutine(Rotate());
        StartCoroutine(Shift());

        #region Local Functions
        void SetupDigitis()
        {
            if(ModeStart == GameModeStart.GameNew)
            {
                CountBombs = _countBombsStart;
                SetCurrentShapes(_startCountShapesForLevel_D, _countShapesPerLevel_D);
                _shapesManager.InitializeNewDigitis(_gameData.MaxDigit, _gameData.ShapeType);
            }
            else
            {
                _shapesManager.InitializeContinueDigitis(_gameData.MaxDigit, _gameData.ShapeType, _gameData.NextShape, _gameData.NextBlocksShape);
            }
            _shapesManager.EventEndMoveDown += OnBlockEndMoveDownDigitis;
        }
        void SetupTetris()
        {
            if (ModeStart == GameModeStart.GameNew)
            {
                SetCurrentShapes(_startCountShapesForLevel_T, _countShapesPerLevel_T);
                _shapesManager.InitializeNewTetris();
            }
            else
            {
                _shapesManager.InitializeContinueTetris(_gameData.NextShape);
            }

            _shapesManager.EventEndMoveDown += OnBlockEndMoveDownTetris;
        }
        #endregion
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

            StartMove(_gameData.IsGravity);
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

            _shapesManager.StartFall(blocks, _gameData.IsDigitis, element.Value.x);
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

            StartMove(false);
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

    private void StartMove(bool isGravity)
    {
        if (_shapesManager.StartMove(isGravity, Level))
        {
            if (--CountShapes == 0)
                LevelUp();
        }
        else
        {
            _shapesManager.EventEndMoveDown -= OnBlockEndMoveDownDigitis;
            _shapesManager.EventEndMoveDown -= OnBlockEndMoveDownTetris;
            StopCoroutine(Rotate());
            StopCoroutine(Shift());
            Debug.Log("Stop");
        }

        void LevelUp()
        {
            Level++;
            if (IsDigitis)
            {
                CountBombs++;
                SetCurrentShapes(_startCountShapesForLevel_D, _countShapesPerLevel_D);
            }
            else
            {
                SetCurrentShapes(_startCountShapesForLevel_T, _countShapesPerLevel_T);
            }
        }
    }

    private void SetCurrentShapes(int startShapes, int perLevel)
    {
        CountShapesMax = startShapes + perLevel * (Level - 1);
        _gameData.CountShapes = CountShapesMax;
    }

    private void OnBomb()
    {
        if (CountBombs <= 0) 
            return;

        if (_shapesManager.ShapeToBomb())
            CountBombs--;
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
