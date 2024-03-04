using Newtonsoft.Json;
using System;
using UnityEngine;

public class DataGame : ASingleton<DataGame>
{
    private const string KEY = "gmd";
    
    [Space]
    [SerializeField] private int _startCountBombs = 1;
    [Space]
    [SerializeField] private int _bombPerLevelBase = 18;
    [SerializeField] private int _bombPerScoreBase = 400;
    [Space]
    [SerializeField] private int _startCountShapes = 16;
    [SerializeField] private int _steepShapesPerLevel = 10;
    [SerializeField] private float[] _shapesPerLevel;
    [Space]
    [SerializeField] private Speeds _speeds;
    [Space]
    [SerializeField] private GameSettings _settings;
    [SerializeField, Range(3, 8)] private int _minDigit = 5;

    private GameSave _data;
    private int _countShapesMax;
    private int _countBombInfinityAdd;

    public Speeds Speeds => _speeds;
    public int CountShapesMax { get => _countShapesMax; set { _countShapesMax = value; EventChangeCountShapesMax?.Invoke(value); } }

    public bool IsNewGame { get => _data.ModeStart == GameModeStart.New; private set => _data.ModeStart = value ? GameModeStart.New : GameModeStart.Continue; }
    public int Level { get => _data.CurrentLevel; private set { _data.CurrentLevel = value; EventChangeLevel?.Invoke(value); } }
    public long Score { get => _data.Score; private set { _data.Score = value; EventChangeScore?.Invoke(value.ToString()); } }
    public long MaxScore { get => _data.MaxScore; private set { _data.MaxScore = value; EventChangeMaxScore?.Invoke(value.ToString()); } }
    public int CountShapes { get => _data.CountShapes; set { _data.CountShapes = value; EventChangeCountShapes?.Invoke(value); } }
    public ShapeSize ShapeType { get => _data.ShapeType; set => _data.ShapeType = value; }
    public bool IsInfinityMode { get => _data.Mode == GameMode.Infinity; set => _data.Mode = value ? GameMode.Infinity : GameMode.Normal; }
    public int MinDigit => _minDigit;
    public int MaxDigit { get => _data.MaxDigit; set => _data.MaxDigit = value; }
    public int CountBombs { get => _data.CountBombs; set { _data.CountBombs = value; EventChangeCountBombs?.Invoke(value); } }
    public ShapeType NextShape { get => _data.NextShape; set => _data.NextShape = value; }
    public int[] NextBlocksShape { get => _data.NextBlocksShape; set => _data.NextBlocksShape = value; }
    public int[,] SaveArea { get => _data.Area; set => _data.Area = value; }

    public event Action<int> EventChangeLevel;
    public event Action<int> EventChangeCountShapes;
    public event Action<int> EventChangeCountShapesMax;
    public event Action<string> EventChangeScore;
    public event Action<string> EventChangeMaxScore;
    public event Action<int> EventChangeCountBombs;

    public bool Initialize(bool isLoad)
    {
        bool result = false;
        if (isLoad)
            result = Load();

        if (!result)
            _data = new(_settings, _startCountBombs);

        return result;
    }

    private bool Load()
    {
        Return<GameSave> data = Storage.Load<GameSave>(KEY);
        if (data.Result)
            _data = data.Value;

        return data.Result;
    }

    public void Save(bool isSaveHard, Action<bool> callback) => Storage.Save(KEY, _data, isSaveHard, callback);

    public void StartGame()
    {
        _data.ModeStart = GameModeStart.Continue;
        _countBombInfinityAdd = Mathf.FloorToInt(Score / (CalkRateBomb() * _bombPerScoreBase)) + 1;
        _speeds.Initialize(IsInfinityMode, Level);

    }
    public void LevelUp()
    {
        Level++;
        _speeds.SetSpeed(Level);
        CountBombs += Level % CalkRateBomb() == 0 ? 1 : 0;
        CountShapes = CalkMaxShapes();
    }

    public void ResetData()
    {
        _data.Reset(_startCountBombs);
        _countShapesMax = _data.CountShapes = _startCountShapes;
    }

    public void CalkScore(int digit, int countSeries, int countOne)
    {
        Score += digit * (2 * countSeries + countOne - digit);

        if (!IsInfinityMode)
            return;

        if (Score > MaxScore)
            MaxScore = Score;

        if (Score > _countBombInfinityAdd * CalkRateBomb() * _bombPerScoreBase)
        {
            CountBombs++;
            _countBombInfinityAdd++;
        }
    }

    public int CalkMaxShapes()
    {
        if (IsInfinityMode)
            return CountShapesMax = 1;

        CountShapesMax = _startCountShapes;
        int lvl = Level - 1;
        for (int i = 0; i < _shapesPerLevel.Length - 1; i++)
        {
            CountShapesMax += Mathf.RoundToInt(_shapesPerLevel[i] * (lvl < _steepShapesPerLevel ? lvl : _steepShapesPerLevel));
            lvl -= _steepShapesPerLevel;

            if(lvl <= 0)
                return CountShapesMax;
        }
        return CountShapesMax += Mathf.RoundToInt(_shapesPerLevel[^1] * lvl);
    }

    private int CalkRateBomb() => _bombPerLevelBase - ShapeType.ToInt() * 2 - MaxDigit;

    #region Nested Classe
    private class GameSave : GameSettings
    {
        [JsonProperty("gms")]
        public GameModeStart ModeStart { get; set; } = GameModeStart.New;

        [JsonProperty("scr")]
        public long Score { get; set; }
        [JsonProperty("msc")]
        public long MaxScore { get; set; }
        [JsonProperty("csh")]
        public int CountShapes { get; set; }
        [JsonProperty("bmb")]
        public int CountBombs { get; set; } = 3;
        [JsonProperty("nsh")]
        public ShapeType NextShape { get; set; }
        [JsonProperty("nbs")]
        public int[] NextBlocksShape { get; set; } = new int[2];
        [JsonProperty("are")]
        public int[,] Area { get; set; } = new int[0, 0];
               
        [JsonConstructor]
        public GameSave(GameModeStart modeStart, int currentLevel, ShapeSize shapeType, int maxDigit, GameMode typeGame, long score, long maxScore, int countShapes, int countBombs, ShapeType nextShape, int[] nextBlocksShape, int[,] area) 
            : base(currentLevel, shapeType, maxDigit, typeGame)
        {
            ModeStart = modeStart;
            Score = score;
            MaxScore = maxScore;
            CountShapes = countShapes;
            CountBombs = countBombs;
            NextShape = nextShape;
            NextBlocksShape = nextBlocksShape;
            Area = area;
        }
        public GameSave(GameSettings gameSettings, int countBombs) : base(gameSettings) => Initialize(countBombs);

        public void Reset(int countBombs)
        {
            Initialize(countBombs);
            CurrentLevel = 1;
        }

        private void Initialize(int countBombs)
        {
            ModeStart = GameModeStart.New;
            Score = 0;
            Area = new int[0, 0];
            CountBombs = countBombs;
            NextBlocksShape = new int[0];
        }
    }

    [System.Serializable]
    private class GameSettings
    {
        [JsonProperty("clv"), SerializeField]
        private int _currentLevel = 1;
        [JsonProperty("gtp"), SerializeField]
        private GameMode _modeGame = GameMode.Normal;
        [JsonProperty("shp"), SerializeField]
        private ShapeSize _shapeType = ShapeSize.Domino;
        [JsonProperty("mdg"), SerializeField, Range(4, 9)]
        private int _maxDigit = 7;

        [JsonIgnore]
        public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
        [JsonIgnore]
        public ShapeSize ShapeType { get => _shapeType; set => _shapeType = value; }
        [JsonIgnore]
        public int MaxDigit { get => _maxDigit; set => _maxDigit = value; }
        [JsonIgnore]
        public GameMode Mode { get => _modeGame; set => _modeGame = value; }

        public GameSettings(int currentLevel, ShapeSize shapeType, int maxDigit, GameMode modeGame)
        {
            _currentLevel = currentLevel;
            _shapeType = shapeType;
            _maxDigit = maxDigit;
            _modeGame = modeGame;
        }

        public GameSettings(GameSettings gameSettings) : this(gameSettings.CurrentLevel, gameSettings.ShapeType, gameSettings.MaxDigit, gameSettings.Mode)
        {
        }
    }
     #endregion
}
