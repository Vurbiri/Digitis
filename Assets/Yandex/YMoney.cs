using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public partial class YMoney : ASingleton<YMoney>
{
    [Space]
    [Header("Реклама за вознаграждение")]
    [SerializeField] private int _bombsRewardedAd = 3;

    public int BombsRewardedAd => _bombsRewardedAd;

    private MusicSingleton _globalMusic;

    private void Start()
    {
        _globalMusic = MusicSingleton.InstanceF;
    }

    public async UniTask<bool> ShowFullscreenAdv()
    {
        if (!IsInitialize)
            return false;

        taskEndShowFullscreenAdv?.TrySetResult(false);
        taskEndShowFullscreenAdv = new();

        bool result = await ShowAd(taskEndShowFullscreenAdv, ShowFullscreenAdvJS);

        taskEndShowFullscreenAdv = null;
        return result;
    }

    public async UniTask<bool> ShowRewardedVideo()
    {
        if (!IsInitialize)
            return false;

        taskRewardRewardedVideo?.TrySetResult(false);
        taskCloseRewardedVideo?.TrySetResult(false);
        taskRewardRewardedVideo = new();
        taskCloseRewardedVideo = new();

        bool result = await ShowAd(taskRewardRewardedVideo, ShowRewardedVideoJS, false);
        taskRewardRewardedVideo = null;

        return result;
    }

    public async UniTask<bool> AwaitCloseRewardedVideo()
    {
        bool result = await taskCloseRewardedVideo.Task;
        taskCloseRewardedVideo = null;

        //_globalMusic.UnPause();
        return result;
    }

#if !UNITY_EDITOR
    public bool IsInitialize => IsInitializeJS();

    private async UniTask<bool> ShowAd(UniTaskCompletionSource<bool> taskCompletion, Action action, bool isOn = true)
    {
        _globalMusic.Pause();

        action();
        bool result = await taskCompletion.Task;

        if (isOn)
            _globalMusic.UnPause();
        return result;
    }

    public void ShowBannerAdv() => ShowBannerAdvJS();
    public void HideBannerAdv() => HideBannerAdvJS();

#endif
}
