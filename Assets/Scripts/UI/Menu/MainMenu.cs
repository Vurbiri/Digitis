using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuNavigation
{
    [Space]
    [SerializeField] private Button _leaderboard;
    [SerializeField] private Button _review;

    protected YandexSDK _ysdk;

    private void Start()
    {
        _ysdk = YandexSDK.InstanceF;

        ButtonInitialize().Forget();

        async UniTaskVoid ButtonInitialize()
        {
            bool isFirstStart = SettingsGame.Instance.IsFirstStart;

            _leaderboard.interactable = _ysdk.IsLeaderboard;
            _review.interactable = !isFirstStart && _ysdk.IsLogOn && await _ysdk.CanReview();
        }
    }

    public void OnReview()
    {
        _review.interactable = false;
        _ysdk.RequestReview().Forget();
    }
}
