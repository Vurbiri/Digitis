using System.Collections;
using UnityEngine;

public class BlocksExampleUI : MonoBehaviour
{
    [SerializeField] private float _timeStart = 2f;
    [SerializeField] private float _timeShowingBlock = 0.3f;
    [SerializeField] private float _timeShow = 4f;
    [SerializeField] private float _timeRemovingBlock = 0.25f;
    [SerializeField] private float _timePostRemove = 3f;
    [Space]
    [SerializeField] private Blocks[] _blocks;

    private WaitForSecondsRealtime _pauseStart;
    private WaitForSecondsRealtime _pauseShowingBlock;
    private WaitForSecondsRealtime _pauseShow;
    private WaitForSecondsRealtime _pauseRemovingBlock;
    private WaitForSecondsRealtime _pausePostRemove;

    private void Awake()
    {
        foreach (var block in _blocks)
            block.Setup();

        _pauseStart = new(Random.Range(_timeStart/2f, _timeStart));
        _pauseShowingBlock = new(_timeShowingBlock);
        _pauseShow = new(Random.Range(_timeShow/2f, _timeShow));
        _pauseRemovingBlock = new(_timeRemovingBlock);
        _pausePostRemove = new(_timePostRemove);
    }

    private void OnEnable()
    {
        StartCoroutine(Processing());
    }

    private IEnumerator Processing()
    {
        while(true)
        {
            yield return _pauseStart;

            foreach (var block in _blocks)
                yield return block.ShowBlock(_pauseShowingBlock);

            yield return _pauseShow;

            foreach (var block in _blocks)
                yield return block.StartParticle(_pauseRemovingBlock);

            yield return _pausePostRemove;
        }
    }

    private void OnDisable()
    {
        StopCoroutine(Processing());

        foreach (var block in _blocks)
            block.Hide();
    }

    #region Nested Classe
    [System.Serializable]
    private class Blocks
    {
        [SerializeField] private BlockSettings _settings;
        [Space]
        [SerializeField] private BlockDigitUI[] _blocks;

        public void Setup()
        {
            foreach (var block in _blocks)
                block.SetupBlock(_settings);
        }

        public IEnumerator ShowBlock(WaitForSecondsRealtime delay)
        {
            foreach(var block in _blocks)
            {
                block.ShowBlock();
                yield return delay;
            }
        }

        public IEnumerator StartParticle(WaitForSecondsRealtime delay)
        {
            foreach (var block in _blocks)
            {
                block.StartParticle();
                yield return delay;
            }
        }

        public void Hide()
        {
            foreach (var block in _blocks)
                block.Hide();
        }
    }
    #endregion
}
