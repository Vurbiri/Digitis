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

        _game.EventStartGame += () => _music.Play(Music.Game, CalkPitch(_dataGame.Level));
        _game.EventGameOver += OnGameOver;

        _dataGame.EventChangeLevel += SetPitch;

        _inputController.EventPause += SwitchToMenuMusic;
        _inputController.EventUnPause += SwitchToGameMusic;

        _music.Stop();
    }

    private void SetPitch(int level)
    {
        _music.Pitch = CalkPitch(level);
    }

    private void SwitchToGameMusic()
    {
        _music.Switch(Music.Game, CalkPitch(_dataGame.Level));
    }
    private void SwitchToMenuMusic()
    {
        _music.Switch(Music.Menu);
    }

    private void OnGameOver()
    {
        StartCoroutine(GameOverCoroutine());

        IEnumerator GameOverCoroutine()
        {
            yield return new WaitForSecondsRealtime(_game.PauseGameOver);
            SwitchToMenuMusic();
        }
    }

    private float CalkPitch(int level) => Mathf.Clamp(_pitchStart + level * _pitchPerLevel, _pitchStart, _pitchMax);

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeLevel -= SetPitch;

        if (MusicSingleton.Instance != null)
            _music.Stop();
    }
}
