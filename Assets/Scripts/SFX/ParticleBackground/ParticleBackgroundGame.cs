using UnityEngine;

public class ParticleBackgroundGame : AParticleBackground
{
    [Space]
    [SerializeField] private Game _game;

    protected override void Awake()
    {
        base.Awake();
        _game.EventChangeLevel += _ => ReColorParticleSystem();
    }


}
