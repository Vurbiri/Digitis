using System.Collections;
using UnityEngine;

public class BlockBombUI : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleExplode;
    [SerializeField] private ParticleSystem _particleDigit;
    [SerializeField] private GameObject _bomb;
    [Space]
    [SerializeField] private float _timeShow = 4f;
    [SerializeField] private float _timePostRemove = 1.6f;

    private WaitForSecondsRealtime _pauseShow;
    private WaitForSecondsRealtime _pausePostRemove;

    private void Awake()
    {
        _pauseShow = new(Random.Range(_timeShow / 2f, _timeShow));
        _pausePostRemove = new(_timePostRemove);

        _bomb.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(Processing());
    }

    private IEnumerator Processing()
    {
        while (true)
        {
            _bomb.SetActive(true);

            yield return _pauseShow;

            _bomb.SetActive(false);
            _particleExplode.Play();
            _particleDigit.Play();

            yield return _pausePostRemove;
        }
    }


    private void OnDisable()
    {
        StopCoroutine(Processing());

        _bomb.SetActive(false);
        _particleExplode.Clear();
        _particleExplode.Stop();
        _particleDigit.Clear();
        _particleDigit.Stop();
    }
}
