using UnityEngine;

public class ParticleBackgroundGame : AParticleBackground
{
    protected override void Awake()
    {
        base.Awake();
        DataGame.Instance.EventChangeLevel += OnChangeLevel;
    }

    private void OnChangeLevel(string str) => ReColorParticleSystem();

    private void OnDestroy()
    {
        if(DataGame.Instance != null)
            DataGame.Instance.EventChangeLevel -= OnChangeLevel;
    }
}
