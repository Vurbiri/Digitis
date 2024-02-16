using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    
    [SerializeField] private ShapesManager _shapesManager;
    [SerializeField] private AGameController _gameController;

    private Dictionary<int, int> _columns;
    private DataGame _dataGame;

    private const int TIME_COUNTDOWN = 5000;
    private const int PAUSE_LEVELUP = 900;

    private bool _isSave = false;

    private GameModeStart ModeStart { get => _dataGame.ModeStart; set => _dataGame.ModeStart = value; }
    private int Level { get => _dataGame.Level;}
    private int CountBombs { get => _dataGame.CountBombs; set => _dataGame.CountBombs = value; }
    private int CountShapes { get => _dataGame.CountShapes; set => _dataGame.CountShapes = value; }
    private int CountShapesMax { get => _dataGame.CountShapesMax; }

    public event Action EventCountdown;

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
        _gameController.EventPause += OnPause;
        _gameController.EventUnPause += OnUnPause;
    }

    private void Start()
    {
        CountdownAsync().Forget();

        _dataGame.CalkMaxShapes();
        if (ModeStart == GameModeStart.GameNew)
            CountShapes = CountShapesMax;

        _shapesManager.Initialize();
        _shapesManager.EventEndMoveDown += OnBlockEndMoveDown;

        #region Local Functions
        async UniTaskVoid CountdownAsync()
        {
            EventCountdown?.Invoke();
           
            await UniTask.Delay(TIME_COUNTDOWN, true);

            ModeStart = GameModeStart.GameContinue;
            _gameController.ControlEnable = true;

            StartMoveAsync().Forget();
        }
        #endregion
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

            StartMoveAsync().Forget();
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

    private async UniTaskVoid StartMoveAsync()
    {
        if (--CountShapes == 0)
            await LevelUpAsync();

        if (!_shapesManager.StartMove(Level))
        {
            _dataGame.ResetData();
            _shapesManager.EventEndMoveDown -= OnBlockEndMoveDown;
            Debug.Log("Stop");
        }

        #region Local Functions
        UniTask LevelUpAsync()
        {
            _dataGame.LevelUp();

            return UniTask.Delay(PAUSE_LEVELUP);
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

    private void OnPause()
    {
        Time.timeScale = 0.0000001f;
    }
    private void OnUnPause()
    {
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeScore -= OnChangeScore;

        _gameController.EventLeftPress -= _shapesManager.Left;
        _gameController.EventRightPress -= _shapesManager.Right;
        _gameController.EventStartDown -= _shapesManager.StartMoveDown;
        _gameController.EventEndDown -= _shapesManager.EndMoveDown;
        _gameController.EventRotationPress -= _shapesManager.Rotate;
        _gameController.EventBombClick -= OnBomb;
        _gameController.EventPause -= OnPause;
        _gameController.EventUnPause -= OnUnPause;
    }
}
