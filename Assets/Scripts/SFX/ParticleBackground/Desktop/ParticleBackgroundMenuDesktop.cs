public class ParticleBackgroundMenuDesktop : AParticleBackgroundDesktop
{
    protected override void Awake()
    {
        base.Awake();
        _mainModule.duration = _mainModule.startLifetimeMultiplier * _timeRecolor;
        Play();
    }
}
