using UnityEngine;

public class ParticleBackgroundGameMobile : ParticleBackgroundMenuMobile
{
    [Space]
    [SerializeField] private Game _game;

    protected override void Awake()
    {
        base.Awake();

        _game.EventGameOver += OnGameOver;
        _game.EventLeaderboard += (b) => { if (!b) return; StartReColorParticlesCoroutine(); };
    }
}
