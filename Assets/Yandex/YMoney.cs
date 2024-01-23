using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public partial class YMoney : Singleton<YMoney>
{
    [Space]
    [Header("Полноэкранная реклама")]
    [SerializeField] private float _repairFullscreenAd = 3;
    [SerializeField] private int _addLevelFullscreenAd = 3;
    [Space]
    [Header("Реклама за вознаграждение")]
    [SerializeField] private float _repairRewardedAd = 0.35f;
    [SerializeField] private int _addLevelRewardedAd = 3;

    public bool IsFirstStart { get; set; } = true;

    public float RepairFullscreenAd => _repairFullscreenAd;
    public int LevelFullscreenAd { get; private set; }
    public int LevelHardFullscreenAd { get; private set; }

    public float RepairRewardedAd => _repairRewardedAd;
    public int LevelRewardedAd { get; private set; }

    //private GlobalMusic _globalMusic;

    private void Start()
    {
        //_globalMusic = GlobalMusic.InstanceF;
        ResetLevelShowAd();
    }

    public void ResetLevelShowAd()
    {
        LevelFullscreenAd = _addLevelFullscreenAd;
        LevelHardFullscreenAd = _addLevelFullscreenAd * 2;
        LevelRewardedAd = _addLevelRewardedAd;
    }

    public async UniTask<bool> ShowFullscreenAdv(int level)
    {
        if (!IsInitialize)
            return false;

        taskEndShowFullscreenAdv?.TrySetResult(false);
        taskEndShowFullscreenAdv = new();

        bool result = await ShowAd(taskEndShowFullscreenAdv, ShowFullscreenAdvJS);
        if (result)
        {
            LevelFullscreenAd = _addLevelFullscreenAd + level;
            LevelHardFullscreenAd = LevelFullscreenAd + _addLevelFullscreenAd;
        }
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

    public async UniTask<bool> ShowRewardedVideo(int level)
    {
        bool result = await ShowRewardedVideo();
        if (result)
            LevelRewardedAd = _addLevelRewardedAd + level;

        return result;
    }
    public async UniTask<bool> AwaitCloseRewardedVideo()
    {
        bool result = await taskCloseRewardedVideo.Task;
        taskCloseRewardedVideo = null;

       // _globalMusic.UnPause();

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
