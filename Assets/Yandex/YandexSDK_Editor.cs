#if UNITY_EDITOR

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class YandexSDK
{
    public bool IsInitialize => true;
    public bool IsPlayer => true;
    public bool IsLeaderboard => true;
    public string PlayerName => "";
    public bool IsLogOn { set; get; } = true;
    public string Lang => "ru";

    public bool IsDesktop => true;

    public UniTask<bool> InitYsdk() => UniTask.RunOnThreadPool(() => true);
    public void LoadingAPI_Ready() { }
    public UniTask<bool> InitPlayer() => UniTask.RunOnThreadPool(() => true);
    public async UniTask<bool> LogOn()
    {
        await UniTask.Delay(1000, true);
        IsLogOn = true;
        return true;
    }
    public UniTask<bool> InitLeaderboards() => UniTask.RunOnThreadPool(() => false);
    public UniTask<(bool result, Texture texture)> GetPlayerAvatar(AvatarSize size) => UniTask.RunOnThreadPool<(bool result, Texture texture)>(() => (false, null));

    public UniTask<(bool result, LeaderboardResult lbResult)> GetPlayerResult(string lbName) => UniTask.RunOnThreadPool(() => (true, new LeaderboardResult(6, 1010)));
    public UniTask<bool> SetScore(string lbName, int score) => UniTask.RunOnThreadPool(() => true);
    public UniTask<(bool result, Leaderboard table)> GetLeaderboard(string lbName, int quantityTop, bool includeUser = false, int quantityAround = 0, AvatarSize size = AvatarSize.Small)
    {
        List<LeaderboardRecord> list = new();
        list.Add(new(1, 1100, "Седов Герман", ""));
        list.Add(new(2, 1000, "Журавлев Тимофей", ""));
        list.Add(new(3, 900, "Крылов Богдан", ""));
        list.Add(new(4, 800, "Панов Фёдор", ""));
        list.Add(new(5, 600, "Зайцев Илья", ""));
        list.Add(new(6, 550, "Лебедева Алёна", ""));
        list.Add(new(8, 500, "", ""));
        list.Add(new(9, 400, "Муравьев Егор", ""));
        list.Add(new(10, 300, "Казанцев Алексей", ""));
        list.Add(new(11, 200, "Баженов Борис", ""));
        list.Add(new(12, 100, "Крылова Таня", ""));

        Leaderboard l = new(2, list.ToArray());

        return UniTask.RunOnThreadPool(() => (true, l));
    }

    public async UniTask<bool> Save(string key, string data)
    {
        using StreamWriter sw = new(Path.Combine(Application.persistentDataPath, key));
        await sw.WriteAsync(data);

        return true;
    }
    public async UniTask<string> Load(string key)
    {
        string path = Path.Combine(Application.persistentDataPath, key);
        if (File.Exists(path))
        {
            using StreamReader sr = new(path);
            return await sr.ReadToEndAsync();
        }
        return null;
    }

    public UniTask<bool> CanReview() => UniTask.RunOnThreadPool(() => true);
    public async UniTaskVoid RequestReview() => await UniTask.Delay(1); 

    public UniTask<bool> CanShortcut() => UniTask.RunOnThreadPool(() => true);
    public UniTask<bool> CreateShortcut() => UniTask.RunOnThreadPool(() => true);

}
#endif
