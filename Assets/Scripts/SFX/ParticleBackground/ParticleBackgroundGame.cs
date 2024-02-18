using UnityEngine;

public class ParticleBackgroundGame : AParticleBackground
{
    [Space]
    [SerializeField] private Game _game;
    [Space]
    [SerializeField] private Color _colorGameOver;

    protected override void Awake()
    {
        base.Awake();
        DataGame.Instance.EventChangeLevel += OnChangeLevel;
        _game.EventGameOver += OnGameOver;

        void OnGameOver()
        {
            ClearAndStop();
            _mainModule.startColor = _colorGameOver;
            _shapeModule.radiusMode = ParticleSystemShapeMultiModeValue.Random;
            _emissionModule.rateOverTimeMultiplier  *= 2f;
            Play();
        }
    }

    private void OnChangeLevel(int value) => ReColorParticleSystem();

    private void OnDestroy()
    {
        if(DataGame.Instance != null)
            DataGame.Instance.EventChangeLevel -= OnChangeLevel;
    }
}
