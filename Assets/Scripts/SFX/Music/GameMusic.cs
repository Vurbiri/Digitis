using System.Collections;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private AInputController _inputController;
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

        _inputController.EventPause += _music.MenuPlay;
        _inputController.EventUnPause += PlayGameMusic;

        _music.Stop();
    }

    private void SetPitch(int level)
    {
        _music.Pitch = Mathf.Clamp(_pitchStart + level * _pitchPerLevel, _pitchStart, _pitchMax);
    }

    private void PlayGameMusic()
    {
        _music.GamePlay(Mathf.Clamp(_pitchStart + _dataGame.Level * _pitchPerLevel, _pitchStart, _pitchMax));
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

        _game.EventStartGame -= PlayGameMusic;
        _game.EventGameOver -= OnGameOver;
        _inputController.EventUnPause -= PlayGameMusic;
        _inputController.EventPause -= _music.MenuPlay;

        
        
    }
}
