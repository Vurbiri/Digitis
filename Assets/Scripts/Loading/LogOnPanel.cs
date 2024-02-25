using Cysharp.Threading.Tasks;
using UnityEngine;

public class LogOnPanel : MonoBehaviour
{
    private UniTaskCompletionSource<bool> _taskLogOn;
    YandexSDK _ysdk;

    public async UniTask<bool> TryLogOn()
    {
        MusicSingleton music = MusicSingleton.InstanceF;
        _ysdk = YandexSDK.InstanceF;
        
        music.Play(Music.Game);

        _taskLogOn = new();
        gameObject.SetActive(true);

        while (await _taskLogOn.Task)
        {
            if (await Authorization())
                break;

            _taskLogOn = new();
            Message.BannerKey("ErrorLogon", MessageType.Error);
        }

        music.Stop();
        gameObject.SetActive(false);

        return _ysdk.IsLogOn;

        #region Local Functions

        async UniTask<bool> Authorization()
        {
            Message.BannersClear();

            if (!_ysdk.IsPlayer)
                if (!await _ysdk.InitPlayer())
                    return false;

            if (!_ysdk.IsLogOn)
                if (!await _ysdk.LogOn())
                    return false;

            if (!_ysdk.IsLeaderboard)
                await _ysdk.InitLeaderboards();

            return true;

        }
        #endregion
    }

    public void OnGuest()
    {
        _taskLogOn?.TrySetResult(false);
    }

    public void OnLogOn()
    {
        _taskLogOn?.TrySetResult(true);
    }

    private void FixedUpdate()
    {
        if (_ysdk.IsLogOn)
            _taskLogOn?.TrySetResult(true);
    }
}
