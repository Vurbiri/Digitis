using UnityEngine;

public class ParticleBackgroundGameMobile : ParticleBackgroundMenuMobile
{
    [Space]
    [SerializeField] private Game _game;
    [Space]
    [SerializeField] private Color _colorGameOver;

    protected override void Awake()
    {
        base.Awake();
        _game.EventGameOver += OnGameOver;

        void OnGameOver()
        {
            StopCoroutine(ReColorParticlesCoroutine());
            ClearAndStop();
            _mainModule.startColor = _colorGameOver;
            _emissionPerRadius *= 2f;
            _emissionModule.rateOverTimeMultiplier  *= 2f;
            Play();
        }
    }
}
