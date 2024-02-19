using UnityEngine;

public abstract class AParticleBackgroundDesktop : AParticleBackground
{

    protected override void OnReSizeParticleSystem(Vector2 halfSize)
    {
        ClearAndStop();

        _shapeModule.radius = halfSize.x;
        _emissionModule.rateOverTimeMultiplier = halfSize.x * _emissionPerRadius;

        Play();
    }

}
