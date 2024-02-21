using UnityEngine;

public class ParticleBackgroundGameDesktop : AParticleBackgroundDesktop
{
    [Space]
    [SerializeField] private float _speedMin = 0.275f;
    [SerializeField] private float _speedMax = 0.825f;
    [SerializeField] private float _speedPerLevel = 0.011f;
    [Space]
    [SerializeField] private Game _game;
    [Space]
    [SerializeField] private Color _colorGameOver;
    [SerializeField] private float _speedGameOver = 0.6f;

    protected override void Awake()
    {
        DataGame dataGame = DataGame.Instance;

        base.Awake();
        SetSpeed(Mathf.Clamp(_speedMin + _speedPerLevel * dataGame.Level, _speedMin, _speedMax));
        _mainModule.duration = _mainModule.startLifetimeMultiplier * _timeRecolor;
        Play();

        dataGame.EventChangeLevel += OnChangeLevel;
        _game.EventGameOver += OnGameOver;

        void OnGameOver()
        {
            StopCoroutine(ReColorParticlesCoroutine());
            ClearAndStop();
            SetSpeed(_speedGameOver);
            _mainModule.startColor = _colorGameOver;
            _emissionPerRadius *= 2f;
            _emissionModule.rateOverTimeMultiplier *= 2f;
            Play();
        }
    }

    private void OnChangeLevel(int level)
    {
        ClearAndStop();
        SetSpeed(Mathf.Clamp(_speedMin + _speedPerLevel * level, _speedMin, _speedMax));
        Play();
    }

    private void SetSpeed(float speed)
    {
        _mainModule.startSpeedMultiplier = speed;
        _emissionModule.rateOverTimeMultiplier = _cameraSize.Size.x * _emissionPerRadius * speed / 2f;
        _mainModule.startLifetimeMultiplier = Mathf.Ceil(_cameraSize.Size.y / speed);
        _mainModule.duration = _mainModule.startLifetimeMultiplier * _timeRecolor;
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            DataGame.Instance.EventChangeLevel -= OnChangeLevel;
    }
}
