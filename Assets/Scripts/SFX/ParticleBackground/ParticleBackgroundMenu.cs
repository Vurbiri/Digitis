using System.Collections;
using UnityEngine;

public class ParticleBackgroundMenu : AParticleBackground
{
    [Space]
    [SerializeField] private float _timeUpdateReColor = 300f;

    protected override void Awake()
    {
        base.Awake();

        WaitForSecondsRealtime pause = new(_timeUpdateReColor);
        StartCoroutine(ReColorParticleSystemCoroutine());

        IEnumerator ReColorParticleSystemCoroutine()
        {
            while(true) 
            {
                yield return pause;
                ReColorParticleSystem();
            }
        }
    }
}
