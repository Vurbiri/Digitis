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
    
    private Dictionary<int, int> _columns;
    private DataGame _dataGame;
    
    private bool _isSave = false;

    private GameModeStart ModeStart { get => _dataGame.ModeStart; set => _dataGame.ModeStart = value; }
    private int Level { get => _dataGame.Level;}
    private int CountBombs { get => _dataGame.CountBombs; set => _dataGame.CountBombs = value; }
    private int CountShapes { get => _dataGame.CountShapes; set => _dataGame.CountShapes = value; }
    private int CountShapesMax { get => _dataGame.CountShapesMax; }

    private void Awake()
    {
        _dataGame = DataGame.InstanceF;
        _dataGame.EventChangeScore += OnChangeScore;

        _gameController.EventLeftPress += _shapesManager.Left;
        _gameController.EventRightPress += _shapesManager.Right;
        _gameController.EventStartDown += _shapesManager.StartMoveDown;
        _gameController.EventEndDown += _shapesManager.EndMoveDown;
        _gameController.EventRotationPress += _shapesManager.Rotate;
        _gameController.EventBombClick += OnBomb;
    }

    private void Start()
    {
        _dataGame.CalkMaxShapes();
        if (ModeStart == GameModeStart.GameNew)
            CountShapes = CountShapesMax;

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
        _dataGame.Save(isSaveHard, callback);
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
            _dataGame.ResetData();
            _shapesManager.EventEndMoveDown -= OnBlockEndMoveDown;
            StopCoroutine(Rotate());
            StopCoroutine(Shift());
            Debug.Log("Stop");
        }

        #region Local Functions
        void LevelUp()
        {
            _dataGame.LevelUp();
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

    private void OnChangeScore(string str) => _isSave = true;

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

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeScore -= OnChangeScore;
    }
}
