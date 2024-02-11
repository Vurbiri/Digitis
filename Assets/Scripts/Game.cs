using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Game : MonoBehaviour
{
    [SerializeField] private ShapesManager _shapesManager;
    [SerializeField] private AGameController _gameController;
    [Space]
    [SerializeField] private int _startCountShapes = 20;
    [SerializeField] private int _shapesPerLevel = 3;

    private Dictionary<int, int> _columns;
    private GameData _gameData;
    private int _countShapesMax;
    private bool _isSave = false;

    private GameModeStart ModeStart { get => _gameData.ModeStart; set => _gameData.ModeStart = value; }

    public int Level { get => _gameData.CurrentLevel; private set { _gameData.CurrentLevel = value; EventChangeLevel?.Invoke(value.ToString()); } }
    public int Score { get => _gameData.Score; private set { _gameData.Score = value; EventChangePoints?.Invoke(value.ToString()); } }
    public int CountBombs { get => _gameData.CountBombs; private set { _gameData.CountBombs = value; EventChangeCountBombs?.Invoke(value); } }
    public int CountShapes { get => _gameData.CountShapes; private set { _gameData.CountShapes = value; EventChangeCountShapes?.Invoke(value); } }
    public int CountShapesMax { get => _countShapesMax; private set { _countShapesMax = value; EventChangeCountShapesMax?.Invoke(value); } }

    public event Action<string> EventChangeLevel;
    public event Action<string> EventChangePoints;
    public event Action<int> EventChangeCountBombs;
    public event Action<int> EventChangeCountShapes;
    public event Action<int> EventChangeCountShapesMax;

    private void Awake()
    {
        _gameData = GameData.InstanceF;

        _gameController.EventLeftPress += _shapesManager.Left;
        _gameController.EventRightPress += _shapesManager.Right;
        _gameController.EventStartDown += _shapesManager.StartMoveDown;
        _gameController.EventEndDown += _shapesManager.EndMoveDown;
        _gameController.EventRotationPress += _shapesManager.Rotate;
        _gameController.EventBombClick += OnBomb;

        CountShapesMax = CalkMaxShapes();
        if (ModeStart == GameModeStart.GameNew)
            CountShapes = CountShapesMax;
    }

    private void Start()
    {
        _shapesManager.Initialize();
        _shapesManager.EventEndMoveDown += OnBlockEndMoveDown;

        ModeStart = GameModeStart.GameContinue;
        _gameController.ControlEnable = true;

        StartMove();

        //StartCoroutine(Rotate());
        //StartCoroutine(Shift());
    }

    public void Save(bool isSaveHard = true, Action<bool> callback = null)
    {
        _shapesManager.SaveShape();
        _gameData.Save(isSaveHard, callback);
        _isSave = false;
    }

    private void OnBlockEndMoveDown()
    {
        if (FallColumns())
            return;

        OnBlockEndMoveDownAsync().Forget();

        #region Local Functions
        async UniTaskVoid OnBlockEndMoveDownAsync()
        {
            _columns = await _shapesManager.CheckNewBlocksAsync();
            if (FallColumns())
                return;

            if(_isSave)
                Save();

            StartMove();
        }

        bool FallColumns()
        {
            if (_columns == null || _columns.Count == 0)
                return false;

            List<Block> blocks;
            KeyValuePair<int, int> element;

            do
            {
                element = _columns.First(); 
                 _columns.Remove(element.Key);
                blocks = _shapesManager.GetBlocksInColumn(element.Key, element.Value);
            }
            while (blocks.Count == 0 && _columns.Count > 0);

            if(blocks.Count == 0)
                return false;

            _shapesManager.StartFall(blocks);
            return true;
        }
        #endregion
    }

    private void StartMove()
    {
        if (_shapesManager.StartMove(Level))
        {
            if (--CountShapes == 0)
                LevelUp();
        }
        else
        {
            _gameData.ResetData();
            _shapesManager.EventEndMoveDown -= OnBlockEndMoveDown;
            StopCoroutine(Rotate());
            StopCoroutine(Shift());
            Debug.Log("Stop");
        }

        #region Local Functions
        void LevelUp()
        {
            Level++;
            CountBombs++;
            CountShapes = CountShapesMax = CalkMaxShapes();
        }
        #endregion
    }

    private void OnBomb()
    {
        if (CountBombs <= 0)
            return;

        if (_shapesManager.ShapeToBomb())
            CountBombs--;
    }


    public void CalkScore(int digit, int countSeries, int countOne)
    {
        Score += digit * (2 * countSeries + countOne - digit);
        _isSave = true;
    }

    private int CalkMaxShapes() => _startCountShapes + _shapesPerLevel * (Level - 1);

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
}
