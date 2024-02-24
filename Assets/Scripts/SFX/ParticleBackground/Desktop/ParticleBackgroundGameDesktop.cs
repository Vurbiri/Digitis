using UnityEngine;

public class ParticleBackgroundGameDesktop : ParticleBackgroundMenuDesktop
{
    [Space]
    [SerializeField] private Game _game;
    [Space]
    [SerializeField] private float _speedMin = 0.275f;
    [SerializeField] private float _speedMax = 0.825f;
    [SerializeField] private float _speedPerLevel = 0.011f;
    
    protected override void Awake()
    {
        DataGame dataGame = DataGame.Instance;
        base.Awake();

        SetSpeed(CalkSpeed(dataGame.Level));
        dataGame.EventChangeLevel += OnChangeLevel;
        _game.EventGameOver += OnGameOver;
        _game.EventLeaderboard += (b) => { if (!b) return; StartReColorParticlesCoroutine(); };

    }

    private void OnChangeLevel(int level) => SetSpeed(CalkSpeed(level));
    private float CalkSpeed(int level) => Mathf.Clamp(_speedMin + _speedPerLevel * level, _speedMin, _speedMax);

    private void OnDestroy()
    {
        if (DataGame.Instance != null)
            DataGame.Instance.EventChangeLevel -= OnChangeLevel;
    }
}
