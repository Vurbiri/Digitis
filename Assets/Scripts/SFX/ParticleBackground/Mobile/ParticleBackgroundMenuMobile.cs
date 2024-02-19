using System.Collections;
using UnityEngine;

public class ParticleBackgroundMenuMobile : AParticleBackgroundMobile
{
    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(ReColorParticleSystemCoroutine());

        IEnumerator ReColorParticleSystemCoroutine()
        {
            while(true) 
            {
                yield return new WaitForSecondsRealtime(_mainModule.startLifetimeMultiplier);
                ReColorParticleSystem();
            }
        }
    }
}
