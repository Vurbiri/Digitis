#if UNITY_EDITOR

using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public partial class YMoney
{
    public bool IsInitialize => true;

    private async UniTask<bool> ShowAd(UniTaskCompletionSource<bool> taskCompletion, Action action, bool isOn = true)
    {
        //_globalMusic.Pause();

        PauseCallback();
        bool result = await taskCompletion.Task;
        Debug.Log(action.Method.Name + " - " + result);

        //if (isOn)
        //    _globalMusic.UnPause();

        return result;

        async void PauseCallback()
        {
            await UniTask.Delay(500, true);
            taskCompletion.TrySetResult(true);
            await UniTask.Delay(100, true);
            taskCloseRewardedVideo?.TrySetResult(true);
        }
    }

    public void ShowBannerAdv() { }
    public void HideBannerAdv() { }
}
#endif