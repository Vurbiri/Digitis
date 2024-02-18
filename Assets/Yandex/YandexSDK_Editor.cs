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
    public UniTask<Return<Texture>> GetPlayerAvatar(AvatarSize size) => UniTask.RunOnThreadPool<Return<Texture>>(() => Return<Texture>.Empty);

    public UniTask<Return<LeaderboardResult>> GetPlayerResult() => UniTask.RunOnThreadPool(() => new Return<LeaderboardResult>(new LeaderboardResult(6, 1)));
    private UniTask<bool> SetScore(int score) => UniTask.RunOnThreadPool(() => true);
    public UniTask<Return<Leaderboard>> GetLeaderboard(int quantityTop, bool includeUser = false, int quantityAround = 0, AvatarSize size = AvatarSize.Small)
    {
        Debug.Log(_lbName);

        List<LeaderboardRecord> list = new()
        {
            new(1, 1100, "����� ������", ""),
            new(2, 1000, "�������� �������", ""),
            new(3, 900, "������ ������", ""),
            new(4, 800, "����� Ը���", ""),
            new(5, 600, "������ ����", ""),
            new(6, 550, "�������� ����", ""),
            new(8, 500, "", ""),
            new(9, 400, "�������� ����", ""),
            new(10, 300, "�������� �������", ""),
            new(11, 200, "������� �����", ""),
            new(12, 100, "������� ����", "")
        };

        Leaderboard l = new(2, list.ToArray());

        return UniTask.RunOnThreadPool(() => new Return<Leaderboard>(l));
    }

    public UniTask<Return<Leaderboard>> GetLeaderboardTest()
    {
        List<LeaderboardRecord> list = new()
        {
            new(1, 1100, "����� ������", ""),
            new(2, 1000, "�������� �������", ""),
            new(3, 900, "������ ������", ""),
            new(4, 800, "����� Ը���", ""),
            new(5, 600, "������ ����", ""),
            new(6, 550, "�������� ����", ""),
            new(7, 500, "", ""),
            new(9, 400, "�������� ����", ""),
            new(10, 300, "�������� �������", ""),
        };

        Leaderboard l = new(2, list.ToArray());

        return UniTask.RunOnThreadPool(() => new Return<Leaderboard>(l));
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
