using UnityEngine;

public class GameMusic : AAudioSourceController
{
    [SerializeField] private Game _game;
    [SerializeField] private AGameController _gameController;
    [Space]
    [SerializeField] private AudioClip _musicGame;
    [SerializeField] private AudioClip _musicMenu;
    [Space]
    [SerializeField] private float _pitchStart = 0.85f;
    [SerializeField] private float _pitchPerLevel = 0.0085f;
    [SerializeField] private float _pitchMax = 1.26f;

    private DataGame _dataGame;

    protected override void Awake()
    {
        base.Awake();
        _dataGame = DataGame.Instance;

        _game.EventStartGame += PlayGameMusic;
        _game.EventGameOver += () => PlayClip(_musicMenu);

        _dataGame.EventChangeLevel += SetPitch;

        _gameController.EventPause += () => PlayClip(_musicMenu);
        _gameController.EventUnPause += PlayGameMusic;

        Stop();
    }

    private void SetPitch(int level)
    {
        float pitch = _pitchStart + level * _pitchPerLevel;
        _thisAudio.pitch = pitch < _pitchMax ? pitch : _pitchMax;
    }

    private void PlayGameMusic()
    {
        float pitch = _pitchStart + _dataGame.Level * _pitchPerLevel;
        PlayClip(_musicGame, 1f, pitch < _pitchMax ? pitch : _pitchMax);
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeLevel -= SetPitch;
    }
}
