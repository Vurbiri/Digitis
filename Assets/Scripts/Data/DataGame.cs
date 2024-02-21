using Newtonsoft.Json;
using System;
using UnityEngine;

public class DataGame : ASingleton<DataGame>
{
    private const string KEY = "gmd";
    
    [Space]
    [SerializeField] private int _startCountBombs = 3;
    [SerializeField] private int _countLeveBomb = 3;
    [Space]
    [SerializeField] private int _startCountShapes = 20;
    [SerializeField] private int _shapesPerLevel = 2;
    [Space]
    [SerializeField] private Speeds _speeds;
    [Space]
    [SerializeField] private GameSettings _settings;

    private GameSave _data;
    private int _countShapesMax;

    public Speeds Speeds => _speeds;
    public int CountShapesMax { get => _countShapesMax; set { _countShapesMax = value; EventChangeCountShapesMax?.Invoke(value); } }

    public GameModeStart ModeStart { get => _data.ModeStart; set => _data.ModeStart = value; }
    public int Level { get => _data.CurrentLevel; set { _data.CurrentLevel = value; EventChangeLevel?.Invoke(value); } }
    public int Score { get => _data.Score; private set { _data.Score = value; EventChangeScore?.Invoke(value.ToString()); } }
    public int CountShapes { get => _data.CountShapes; set { _data.CountShapes = value; EventChangeCountShapes?.Invoke(value); } }
    public ShapeSize ShapeType { get => _data.ShapeType; set => _data.ShapeType = value; }
    public int MaxDigit { get => _data.MaxDigit; set => _data.MaxDigit = value; }
    public int CountBombs { get => _data.CountBombs; set { _data.CountBombs = value; EventChangeCountBombs?.Invoke(value); } }
    public ShapeType NextShape { get => _data.NextShape; set => _data.NextShape = value; }
    public int[] NextBlocksShape { get => _data.NextBlocksShape; set => _data.NextBlocksShape = value; }
    public int[,] SaveArea { get => _data.Area; set => _data.Area = value; }

    public event Action<int> EventChangeLevel;
    public event Action<int> EventChangeCountShapes;
    public event Action<int> EventChangeCountShapesMax;
    public event Action<string> EventChangeScore;
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

    public void LevelUp()
    {
        Level++;
        CountBombs += Level % _countLeveBomb == 0 ? 1 : 0;
        CountShapes = CalkMaxShapes();
    }

    public void ResetData()
    {
        _data.Reset(_startCountBombs);
        CountShapes = CalkMaxShapes();
    }

    public void CalkScore(int digit, int countSeries, int countOne)
    {
        Score += digit * (2 * countSeries + countOne - digit);
    }

    public int CalkMaxShapes() => CountShapesMax = _startCountShapes + _shapesPerLevel * (Level - 1);

    #region Nested Classe
    private class GameSave : GameSettings
    {
        [JsonProperty("mst")]
        public GameModeStart ModeStart { get; set; }

        [JsonProperty("scr")]
        public int Score { get; set; }
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
        public GameSave(GameModeStart modeStart, int currentLevel, ShapeSize shapeType, int maxDigit, int score, int countShapes, int countBombs, ShapeType nextShape, int[] nextBlocksShape, int[,] area) 
            : base(currentLevel, shapeType, maxDigit)
        {
            ModeStart = modeStart;
            Score = score;
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
            ModeStart = GameModeStart.GameNew;
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
        [JsonProperty("shp"), SerializeField]
        private ShapeSize _shapeType = ShapeSize.Domino;
        [JsonProperty("mdg"), SerializeField, Range(3, 9)]
        private int _maxDigit = 7;

        [JsonIgnore]
        public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
        [JsonIgnore]
        public ShapeSize ShapeType { get => _shapeType; set => _shapeType = value; }
        [JsonIgnore]
        public int MaxDigit { get => _maxDigit; set => _maxDigit = value; }

        public GameSettings(int currentLevel, ShapeSize shapeType, int maxDigit)
        {
            _currentLevel = currentLevel;
            _shapeType = shapeType;
            _maxDigit = maxDigit;
        }

        public GameSettings(GameSettings gameSettings) : this(gameSettings.CurrentLevel, gameSettings.ShapeType, gameSettings.MaxDigit)
        {
        }
    }
     #endregion
}
