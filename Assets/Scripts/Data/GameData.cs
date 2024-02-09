using Newtonsoft.Json;
using System;
using UnityEngine;

public class GameData : ASingleton<GameData>
{
    private const string KEY_DIGITIS = "gmd";
    private const string KEY_TETRIS = "gmt";
    
    [Space]
    [SerializeField] private bool _isDigitis = true;
    [Header("Digitis")]
    [SerializeField] private int _countBombsStart = 3;
    [SerializeField] private GameDigitisSave _dataDigitis;
    [Header("Tetris")]
    [SerializeField] private GameTetrisSave _dataTetris;

    private GameTetrisSave _currentData;

    public bool IsDigitis => _isDigitis;

    public GameModeStart ModeStart { get => _currentData.modeStart; set => _currentData.modeStart = value; }
    public int CurrentLevel { get => _currentData.currentLevel; set => _currentData.currentLevel = value; }
    public int Score { get => _currentData.score; set => _currentData.score = value; }
    public int CountShapes { get => _currentData.countShapes; set => _currentData.countShapes = value; }
    public ShapeSize ShapeType { get => _dataDigitis.shapeType; set => _dataDigitis.shapeType = value; }
    public int MaxDigit { get => _dataDigitis.maxDigit; set => _dataDigitis.maxDigit = value; }
    public bool IsGravity { get => _currentData.IsGravity; set => _currentData.IsGravity = value; }
    public int CountBombs { get => _currentData.CountBombs; set => _currentData.CountBombs = value; }
    public ShapeType NextShape { get => _currentData.nextShape; set => _currentData.nextShape = value; }
    public int[] NextBlocksShape { get => _dataDigitis.nextBlocksShape; set => _dataDigitis.nextBlocksShape = value; }
    public int[,] Area { get => _currentData.area; set => _currentData.area = value; }

    public bool Initialize(bool isLoad)
    {
        bool result = false;
        if (isLoad)
            result = Load();
        SetGameType(_isDigitis);
        return result;
    }

    private bool Load()
    {
        ReturnValue<GameDigitisSave> dataDigitis = Storage.Load<GameDigitisSave>(KEY_DIGITIS);
        if (dataDigitis.Result)
            _dataDigitis = dataDigitis.Value;
        ReturnValue<GameTetrisSave> dataTetris = Storage.Load<GameTetrisSave>(KEY_TETRIS);
        if (dataTetris.Result)
            _dataTetris = dataTetris.Value;

        return dataDigitis.Result || dataTetris.Result;
    }

    public void SaveDigitis(bool isSaveHard, Action<bool> callback) => Storage.Save(KEY_DIGITIS, _dataDigitis, isSaveHard, callback);
    public void SaveTetris(bool isSaveHard, Action<bool> callback) => Storage.Save(KEY_TETRIS, _dataTetris, isSaveHard, callback);

    public void ResetData()
    {
        ModeStart = GameModeStart.GameNew;
        CurrentLevel = 1;
        Score = 0;
        Area = new int[0, 0];
        if (IsDigitis)
        {
            CountBombs = _countBombsStart;
            NextBlocksShape = new int[0];
        }
    }

    public void SetGameType(bool isDigitis)
    {  
        _isDigitis = isDigitis;
        _currentData = isDigitis ? _dataDigitis : _dataTetris;
    }
    
    #region Nested Classe
    [System.Serializable]
    private class GameDigitisSave : GameTetrisSave
    {
        [JsonProperty("shp")]
        public ShapeSize shapeType = ShapeSize.Domino;
        [JsonProperty("mdg")]
        [Range(3, 9)] public int maxDigit = 7;
        [JsonProperty("isg"), SerializeField]
        private bool _isGravity = true;
        [JsonProperty("bmb"), SerializeField]
        private int _countBombs;
        [JsonProperty("nbs")]
        public int[] nextBlocksShape;

        [JsonIgnore]
        public override bool IsGravity { get => _isGravity; set => _isGravity = value; }
        [JsonIgnore]
        public override int CountBombs { get => _countBombs; set => _countBombs = value; }

        public GameDigitisSave() : base() { }

        [JsonConstructor]
        public GameDigitisSave(GameModeStart modeStart, int currentLevel, int countShapes, ShapeType nextShape, int score, int[,] area, ShapeSize shapeType, int maxDigit, bool isGravity, int countBombs, int[] nextBlocksShape) 
            : base(modeStart, currentLevel, countShapes, nextShape, score, area)
        {
            this.shapeType = shapeType;
            this.maxDigit = maxDigit;
            _isGravity = isGravity;
            _countBombs = countBombs;
            this.nextBlocksShape = nextBlocksShape;
        }
    }

    [System.Serializable]
    private class GameTetrisSave
    {
        [JsonProperty("mst")]
        public GameModeStart modeStart = GameModeStart.GameNew;
        [JsonProperty("clv")]
        public int currentLevel = 1;
        [JsonProperty("csh")]
        public int countShapes;
        [JsonProperty("nsh")]
        public ShapeType nextShape;
        [JsonProperty("scr")]
        public int score;
        [JsonProperty("are")]
        public int[,] area = new int[0, 0];
        
        [JsonIgnore]
        public virtual bool IsGravity { get => false; set { } }
        [JsonIgnore]
        public virtual int CountBombs { get => 0; set { } }

        public GameTetrisSave() { }

        [JsonConstructor]
        public GameTetrisSave(GameModeStart modeStart, int currentLevel, int countShapes, ShapeType nextShape, int score, int[,] area)
        {
            this.modeStart = modeStart;
            this.currentLevel = currentLevel;
            this.countShapes = countShapes;
            this.nextShape = nextShape;
            this.score = score;
            this.area = area;
        }
    }
    #endregion
}
