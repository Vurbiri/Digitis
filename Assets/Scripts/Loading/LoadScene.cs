using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Collections.Specialized.BitVector32;

public class LoadScene 
{
    private AsyncOperation _asyncOperation = null;
    private readonly int _nextScene;

    public float Progress { get; private set; }

    private event Action<float> EventProgress;
    public LoadScene(int nextScene) => _nextScene = nextScene;

    public void Start(Action<float> action = null)
    {
        _asyncOperation = SceneManager.LoadSceneAsync(_nextScene);
        _asyncOperation.allowSceneActivation = false;
        if (action != null)
        {
            EventProgress = action;
            ProgressAsync().Forget();
        }

        async UniTaskVoid ProgressAsync()
        {
            while (!_asyncOperation.isDone)
            {
                Progress = _asyncOperation.progress * 1.11f;
                EventProgress.Invoke(Progress);
                await UniTask.Yield();
            }
        }
    }

    public void End()
    {
        if (_asyncOperation != null)
            _asyncOperation.allowSceneActivation = true;

        if (EventProgress != null)
        {
            Progress = 1f;
            EventProgress.Invoke(Progress);
            EventProgress = null;
        }
    }
}
