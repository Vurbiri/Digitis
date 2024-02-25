using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MenuNavigation
{
    [Scene]
    [SerializeField] private int _sceneNext = 3;
    [Space]
    [SerializeField] private LeaderboardUI _leaderboard;
    [Space]
    [SerializeField] private ToggleFullInteractable _toggleContinue;
    [Space]
    [SerializeField] private Slider _sliderSize;
    [SerializeField] private Slider _sliderMax;
    [Space]
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private RewardAdPanel _rewardAdPanel;

    private DataGame _dataGame;
    private YMoney _money;

    private void Start()
    {
        _dataGame = DataGame.Instance;
        _money = YMoney.Instance;

        _rewardAdPanel.Initialize();

        _sliderSize.value = _dataGame.ShapeType.ToInt();
        _sliderMax.minValue = _dataGame.MinDigit;
        _sliderMax.value = _dataGame.MaxDigit;

        SetStartInteractable(_dataGame.ModeStart == GameModeStart.GameContinue);
        _toggleContinue.OnValueChanged.AddListener(SetInvertShow);

        #region Local Functions
        void SetStartInteractable(bool value)
        {
            _toggleContinue.Interactable = value;
            _toggleContinue.IsOn = value;
            SetInvertShow(value);
        }
        void SetInvertShow(bool value)
        {
            value = !value;

            _settingsPanel.SetActive(value);
            _rewardAdPanel.Show = value;
        }
        #endregion
    }

    public void OnStart()
    {
        OnStartAsync().Forget();

        async UniTaskVoid OnStartAsync()
        {
            LoadScene loadScene = new(_sceneNext);
            loadScene.Start();

            if (!_toggleContinue.IsOn)
            {
                if (_dataGame.ModeStart == GameModeStart.GameContinue && YandexSDK.Instance.IsLeaderboard)
                    if(await YandexSDK.Instance.TrySetScore(_dataGame.Score))
                        _leaderboard.TryReward().Forget();

                _dataGame.ResetData();
                _dataGame.ShapeType = Mathf.RoundToInt(_sliderSize.value).ToEnum<ShapeSize>();
                _dataGame.MaxDigit = Mathf.RoundToInt(_sliderMax.value);

                bool result = _rewardAdPanel.IsOn;
                if (result)
                    result = await OnShowAdAsync();

                if (!result)
                    _dataGame.Save(true, null);
            }

            MusicSingleton.Instance.Stop();
            loadScene.End();
        }

        async UniTask<bool> OnShowAdAsync()
        {
            bool result = false;
            if (await _money.ShowRewardedVideo())
            {
                result = await _dataGame.AddBonusAd(_money.BombsRewardedAd);
                await _money.AwaitCloseRewardedVideo();
                if (result)
                {
                    Message.BannerKeyFormat("BombsAdd", _money.BombsRewardedAd, time: 4000);
                    await UniTask.Delay(1000, true);
                }
            }
            return result;
        }
    }
        
}
