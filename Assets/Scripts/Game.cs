using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    
    [SerializeField] private ShapesManager _shapesManager;
    [SerializeField] private AInputController _inputController;
    [Space]
    [SerializeField] private int _savePerShapes = 3;

    private Dictionary<int, int> _columns;
    private DataGame _dataGame;

    private int _countSave = 0;
    private bool _isSave = false;

    private const int TIME_COUNTDOWN = 5100;
    private const int PAUSE_LEVELUP = 1150;
    private const int TIME_UNPAUSE = 250;
    private const int PAUSE_GAMEOVER = 1500;

    private GameModeStart ModeStart { get => _dataGame.ModeStart; set => _dataGame.ModeStart = value; }
    private int Level { get => _dataGame.Level;}
    private int CountBombs { get => _dataGame.CountBombs; set => _dataGame.CountBombs = value; }
    private int CountShapes { get => _dataGame.CountShapes; set => _dataGame.CountShapes = value; }
    private int CountShapesMax { get => _dataGame.CountShapesMax; }

    public float PauseGameOver => PAUSE_GAMEOVER / 1000f;


    public event Action EventCountdown;
    public event Action EventStartGame;
    public event Action EventGameOver;
    public event Action EventLeaderboard;

    private void Awake()
    {
        _dataGame = DataGame.InstanceF;
        _dataGame.EventChangeScore += OnChangeScore;

        _inputController.EventLeftPress += _shapesManager.Left;
        _inputController.EventRightPress += _shapesManager.Right;
        _inputController.EventStartDown += _shapesManager.StartMoveDown;
        _inputController.EventEndDown += _shapesManager.EndMoveDown;
        _inputController.EventRotationPress += _shapesManager.Rotate;
        _inputController.EventBombClick += OnBomb;
        _inputController.EventPause += OnPause;
        _inputController.EventUnPause += OnUnPause;
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
            _inputController.ControlEnable = true;

            EventStartGame?.Invoke();

            StartMoveAsync().Forget();
        }
        #endregion
    }

    public void Save(bool isSaveHard = true, Action<bool> callback = null)
    {
        _shapesManager.SaveShape();
        _dataGame.Save(isSaveHard, callback);
        _countSave = 0;
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
        if (CountShapes == 0)
            await LevelUpAsync();
        else if(++_countSave >= _savePerShapes)
            _isSave = true;

        if (_shapesManager.StartMove(Level))
            CountShapes--;
        else
            GameOver().Forget();

        #region Local Functions
        UniTask LevelUpAsync()
        {
            _dataGame.LevelUp();
            Save();
            return UniTask.Delay(PAUSE_LEVELUP);
        }
        async UniTaskVoid GameOver()
        {
            _shapesManager.EventEndMoveDown -= OnBlockEndMoveDown;
            _inputController.ControlEnable = false;

            bool isLeaderboard = await YandexSDK.Instance.TrySetScore(_dataGame.Score);
            _dataGame.ResetData();
            Save();
            EventGameOver?.Invoke();

            await UniTask.Delay(PAUSE_GAMEOVER);
            await _shapesManager.RemoveAll();

            if(isLeaderboard)
                EventLeaderboard?.Invoke();
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
        _inputController.ControlEnable = false;
        Time.timeScale = 0.0000001f;
    }
    private void OnUnPause()
    {
        OnUnPauseAsync().Forget();

        #region Local Functions
        async UniTaskVoid OnUnPauseAsync()
        {
            await UniTask.Delay(TIME_UNPAUSE, true);
            _inputController.ControlEnable = true;
            Time.timeScale = 1f;
        }
        #endregion
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeScore -= OnChangeScore;

        _inputController.EventLeftPress -= _shapesManager.Left;
        _inputController.EventRightPress -= _shapesManager.Right;
        _inputController.EventStartDown -= _shapesManager.StartMoveDown;
        _inputController.EventEndDown -= _shapesManager.EndMoveDown;
        _inputController.EventRotationPress -= _shapesManager.Rotate;
        _inputController.EventBombClick -= OnBomb;
        _inputController.EventPause -= OnPause;
        _inputController.EventUnPause -= OnUnPause;
    }
}
