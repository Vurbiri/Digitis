using UnityEngine;

public class ParticleBackgroundMenuDesktop : AParticleBackground
{
    protected override void OnReSizeParticleSystem(Vector2 halfSize)
    {
        ClearAndStop();

        float speed = _mainModule.startSpeedMultiplier;

        _shapeModule.radius = halfSize.x;
        _emissionModule.rateOverTimeMultiplier = halfSize.x * _emissionPerRadius * speed;
        _mainModule.startLifetimeMultiplier = Mathf.Ceil(2f * halfSize.y / speed);

        Play();
    }
}
