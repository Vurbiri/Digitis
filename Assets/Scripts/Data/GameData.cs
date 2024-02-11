using Newtonsoft.Json;
using System;
using UnityEngine;

public class GameData : ASingleton<GameData>
{
    private const string KEY = "gmd";
    
    [SerializeField] private int _countBombsStart = 3;
    [SerializeField] private GameDigitisSave _dataDigitis;

    public GameModeStart ModeStart { get => _dataDigitis.ModeStart; set => _dataDigitis.ModeStart = value; }
    public int CurrentLevel { get => _dataDigitis.CurrentLevel; set => _dataDigitis.CurrentLevel = value; }
    public int Score { get => _dataDigitis.Score; set => _dataDigitis.Score = value; }
    public int CountShapes { get => _dataDigitis.CountShapes; set => _dataDigitis.CountShapes = value; }
    public ShapeSize ShapeType { get => _dataDigitis.ShapeType; set => _dataDigitis.ShapeType = value; }
    public int MaxDigit { get => _dataDigitis.MaxDigit; set => _dataDigitis.MaxDigit = value; }
    public int CountBombs { get => _dataDigitis.CountBombs; set => _dataDigitis.CountBombs = value; }
    public ShapeType NextShape { get => _dataDigitis.NextShape; set => _dataDigitis.NextShape = value; }
    public int[] NextBlocksShape { get => _dataDigitis.NextBlocksShape; set => _dataDigitis.NextBlocksShape = value; }
    public int[,] Area { get => _dataDigitis.Area; set => _dataDigitis.Area = value; }

    public bool Initialize(bool isLoad)
    {
        bool result = false;
        if (isLoad)
            result = Load();
#if !UNITY_EDITOR
        if (!result)
            _dataDigitis.Reset(_countBombsStart);
#endif

        return result;
    }

    private bool Load()
    {
        ReturnValue<GameDigitisSave> data = Storage.Load<GameDigitisSave>(KEY);
        if (data.Result)
            _dataDigitis = data.Value;

        return data.Result;
    }

    public void Save(bool isSaveHard, Action<bool> callback) => Storage.Save(KEY, _dataDigitis, isSaveHard, callback);

    public void ResetData()
    {
        _dataDigitis.Reset(_countBombsStart);
    }

    #region Nested Classe
    [System.Serializable]
    private class GameDigitisSave
    {
        [JsonProperty("mst")]
        public GameModeStart ModeStart { get; set; }

        [JsonProperty("clv"), SerializeField]
        private int _currentLevel = 1;
        [JsonProperty("shp"), SerializeField]
        private ShapeSize _shapeType = ShapeSize.Domino;
        [JsonProperty("mdg"), SerializeField, Range(3, 9)]
        private int _maxDigit = 7;

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
        

        [JsonIgnore]
        public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
        [JsonIgnore]
        public ShapeSize ShapeType { get => _shapeType; set => _shapeType = value; }
        [JsonIgnore]
        public int MaxDigit { get => _maxDigit; set => _maxDigit = value; }

        public GameDigitisSave() { }

        [JsonConstructor]
        public GameDigitisSave(GameModeStart modeStart, int currentLevel, ShapeSize shapeType, int maxDigit, int score, int countShapes, int countBombs, ShapeType nextShape, int[] nextBlocksShape, int[,] area) 
        {
            ModeStart = modeStart;
            _currentLevel = currentLevel;
            _shapeType = shapeType;
            _maxDigit = maxDigit;
            Score = score;
            CountShapes = countShapes;
            CountBombs = countBombs;
            NextShape = nextShape;
            NextBlocksShape = nextBlocksShape;
            Area = area;
        }

        public void Reset(int countBomb)
        {
            ModeStart = GameModeStart.GameNew;
            CurrentLevel = 1;
            Score = 0;
            Area = new int[0, 0];
            CountBombs = countBomb;
            NextBlocksShape = new int[0];
        }
    }

    #endregion
}
