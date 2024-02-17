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
    }

    private void OnChangeLevel(int value) => ReColorParticleSystem();

    private void OnDestroy()
    {
        if(DataGame.Instance != null)
            DataGame.Instance.EventChangeLevel -= OnChangeLevel;
    }
}
