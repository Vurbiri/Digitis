using System.Collections;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private AGameController _gameController;
    [Space]
    [SerializeField] private float _pitchStart = 0.85f;
    [SerializeField] private float _pitchPerLevel = 0.0085f;
    [SerializeField] private float _pitchMax = 1.26f;

    private DataGame _dataGame;
    private MusicSingleton _music;
      

    protected void Awake()
    {
        _dataGame = DataGame.Instance;
        _music = MusicSingleton.Instance;

        _game.EventStartGame += PlayGameMusic;
        _game.EventGameOver += OnGameOver;

        _dataGame.EventChangeLevel += SetPitch;

        _gameController.EventPause += _music.MenuPlay;
        _gameController.EventUnPause += PlayGameMusic;

        _music.Stop();
    }

    private void SetPitch(int level)
    {
        float pitch = _pitchStart + level * _pitchPerLevel;
        _music.Pitch = pitch < _pitchMax ? pitch : _pitchMax;
    }

    private void PlayGameMusic()
    {
        float pitch = _pitchStart + _dataGame.Level * _pitchPerLevel;
        _music.GamePlay(pitch < _pitchMax ? pitch : _pitchMax);
    }

    private void OnGameOver()
    {
        StartCoroutine(GameOverCoroutine());

        IEnumerator GameOverCoroutine()
        {
            yield return new WaitForSecondsRealtime(_game.PauseGameOver);
            _music.MenuPlay();
        }
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeLevel -= SetPitch;

        if (MusicSingleton.Instance == null)
            return;

        _music.Stop();

        _game.EventStartGame += PlayGameMusic;
        _game.EventGameOver += OnGameOver;
        _gameController.EventUnPause += PlayGameMusic;
        _gameController.EventPause += _music.MenuPlay;

        
        
    }
}
