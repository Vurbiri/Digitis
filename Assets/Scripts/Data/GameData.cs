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
    [SerializeField] private GameDigitisSave _dataDigitis;
    [Header("Tetris")]
    [SerializeField] private GameTetrisSave _dataTetris;

    private GameTetrisSave _currentData;

    public bool IsDigitis => _isDigitis;
    public GameModeStart ModeStart { get => _currentData.modeStart; set => _currentData.modeStart = value; }
    public ShapeSize ShapeType { get => _dataDigitis.shapeType; set => _dataDigitis.shapeType = value; }
    public int MaxDigit { get => _dataDigitis.maxDigit; set => _dataDigitis.maxDigit = value; }
    public bool IsGravity { get => _dataDigitis.isGravity; set => _dataDigitis.isGravity = value; }
    public int CurrentLevel { get => _currentData.currentLevel; set => _currentData.currentLevel = value; }
    public int CountShapes { get => _currentData.countShapes; set => _currentData.countShapes = value; }
    public int CountBombs { get => _getCountBombs(); set => _setCountBombs(value); }

    public ShapeType NextShape => _currentData.nextShape;
    public int[] NextBlocksShape => _dataDigitis.nextBlocksShape;
    public int[,] Area => _currentData.area;

    private Func<int> _getCountBombs;
    private Action<int> _setCountBombs;

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

    public void SetGameType(bool isDigitis)
    {  
        _isDigitis = isDigitis;

        if(isDigitis)
        {
            _currentData = _dataDigitis;
            _getCountBombs = () => _dataDigitis.countBombs;
            _setCountBombs = (b) => _dataDigitis.countBombs = b;
        }
        else
        {
            _currentData = _dataTetris;
            _getCountBombs = () => 0;
            _setCountBombs = _ => { };
        }

    }

    #region Nested Classe
    [System.Serializable]
    private class GameDigitisSave : GameTetrisSave
    {
        [JsonProperty("shp")]
        public ShapeSize shapeType = ShapeSize.Domino;
        [JsonProperty("mdg")]
        [Range(3, 9)] public int maxDigit = 7;
        [JsonProperty("isg")]
        public bool isGravity = true;
        [JsonProperty("bmb")]
        public int countBombs;
        [JsonProperty("nbs")]
        public int[] nextBlocksShape;

        [JsonConstructor]
        public GameDigitisSave(GameModeStart modeStart, int currentLevel, int countShapes, ShapeType nextShape, int[,] area, ShapeSize shapeType, int maxDigit, bool isGravity, int countBombs, int[] nextBlocksShape) 
            : base(modeStart, currentLevel, countShapes, nextShape, area)
        {
            this.shapeType = shapeType;
            this.maxDigit = maxDigit;
            this.isGravity = isGravity;
            this.countBombs = countBombs;
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
        [JsonProperty("ar")]
        public int[,] area;

        [JsonConstructor]
        public GameTetrisSave(GameModeStart modeStart, int currentLevel, int countShapes, ShapeType nextShape, int[,] area)
        {
            this.modeStart = modeStart;
            this.currentLevel = currentLevel;
            this.countShapes = countShapes;
            this.nextShape = nextShape;
            this.area = area;
        }
    }
    #endregion
}
