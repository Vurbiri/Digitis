using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public partial class YandexSDK : Singleton<YandexSDK>
{
#if !UNITY_EDITOR
    public bool IsInitialize => IsInitializeJS();
    public bool IsPlayer => IsPlayerJS();
    public bool IsLogOn => IsLogOnJS();
    public bool IsLeaderboard => IsLeaderboardJS();
    public bool IsDesktop => IsDesktopJS();
    public bool IsMobile => IsMobileJS();

    public string PlayerName => GetPlayerNameJS();
    public UniTask<(bool result, Texture texture)> GetPlayerAvatar(AvatarSize size)
    {
        string url = GetPlayerAvatarURLJS(size.ToString().ToLower());
        return Storage.TryLoadTextureWeb(url);
    }
    public string Lang => GetLangJS();

    public async UniTask<bool> InitYsdk()
    {
        bool result = await WaitTask(ref taskEndInitYsdk, InitYsdkJS);
        taskEndInitYsdk = null;
        return result;
    }
    public void LoadingAPI_Ready() => ReadyJS();
    public async UniTask<bool> InitPlayer()
    {
        bool result = await WaitTask(ref taskEndInitPlayer, InitPlayerJS);
        taskEndInitPlayer = null;
        return result;
    }
    public async UniTask<bool> LogOn()
    {
        bool result = await WaitTask(ref taskEndLogOn, LogOnJS);
        taskEndLogOn = null;
        return result;
    }

    public async UniTask<bool> InitLeaderboards()
    {
        bool result = await WaitTask(ref taskEndInitLeaderboards, InitLeaderboardsJS);
        taskEndInitLeaderboards = null;
        return result;
    }
    public async UniTask<(bool result, LeaderboardResult lbResult)> GetPlayerResult(string lbName) 
    {
        string json = await WaitTask(ref taskEndGetPlayerResult, GetPlayerResultJS, lbName);
        taskEndGetPlayerResult = null;
        if (string.IsNullOrEmpty(json))
            return (false, null);
        else
            return Storage.Deserialize<LeaderboardResult>(json);
    }
    public async UniTask<bool> SetScore(string lbName, int score)
    {
        bool result = await WaitTask(ref taskEndSetScore, SetScoreJS, lbName, score);
        taskEndSetScore = null;
        return result;
    }
    public async UniTask<(bool result, Leaderboard table)> GetLeaderboard(string lbName, int quantityTop, bool includeUser = false, int quantityAround = 1, AvatarSize size = AvatarSize.Medium)
    {
        taskEndGetLeaderboard.TrySetResult(default);
        taskEndGetLeaderboard = new();
        GetLeaderboardJS(lbName, quantityTop, includeUser, quantityAround, size.ToString().ToLower());
        string json = await taskEndGetLeaderboard.Task;
        taskEndGetLeaderboard = null;
        return Storage.Deserialize<Leaderboard>(json);
    }

    public async UniTask<bool> Save(string key, string data) 
    {
        bool result = await WaitTask(ref taskEndSave, SaveJS, key, data);
        taskEndSave = null;
        return result;
    }
    public async UniTask<string> Load(string key) 
    {
        string json = await WaitTask(ref taskEndLoad, LoadJS, key);
        taskEndLoad = null;
        return json;
    }

    public async UniTask<bool> CanReview()
    {
        bool result = await WaitTask(ref taskEndCanReview, CanReviewJS);
        taskEndCanReview = null;
        return result;
    }
    public async UniTaskVoid RequestReview()
    {
        await WaitTask(ref taskEndRequestReview, RequestReviewJS);
        taskEndRequestReview = null;
    }

    public async UniTask<bool> CanShortcut()
    {
        bool result = await WaitTask(ref taskEndCanShortcut, CanShortcutJS);
        taskEndCanShortcut = null;
        return result;
    }
    public async UniTask<bool> CreateShortcut() 
    {
        bool result = await WaitTask(ref taskEndCreateShortcut, CreateShortcutJS);
        taskEndCreateShortcut = null;
        return result;
    }
#endif

private UniTask<T> WaitTask<T>(ref UniTaskCompletionSource<T> taskCompletion, Action action)
    {
        taskCompletion?.TrySetResult(default);
        taskCompletion = new();
        action();
        return taskCompletion.Task;
    }
    private UniTask<T> WaitTask<T, U>(ref UniTaskCompletionSource<T> taskCompletion, Action<U> action, U value)
    {
        taskCompletion?.TrySetResult(default);
        taskCompletion = new();
        action(value);
        return taskCompletion.Task;
    }
    private UniTask<T> WaitTask<T, U, V>(ref UniTaskCompletionSource<T> taskCompletion, Action<U, V> action, U value1, V value2)
    {
        taskCompletion?.TrySetResult(default);
        taskCompletion = new();
        action(value1, value2);
        return taskCompletion.Task;
    }
}

