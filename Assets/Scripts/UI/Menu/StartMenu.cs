using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MenuNavigation
{
    [Scene]
    [SerializeField] private int _sceneNext = 3;
    [Space]
    [SerializeField] private LeaderboardUI _leaderboard;
    [Space]
    [SerializeField] private ToggleFullInteractable _toggleContinue;
    [Space]
    [SerializeField] private SliderFullInteractable _sliderSize;
    [SerializeField] private SliderFullInteractable _sliderMax;
    [Space]
    [SerializeField] private RewardAdPanel _rewardAdPanel;

    private DataGame _dataGame;
    private YMoney _money;

    private void Start()
    {
        _dataGame = DataGame.Instance;
        _money = YMoney.Instance;

        _rewardAdPanel.Initialize();

        //_toggleContinue = _continue.Slider as Toggle;
        //_sliderSize = _sliderSize.Slider as Slider;
        //_sliderMax = _sliderMax.Slider as Slider;

        _sliderSize.Value = _dataGame.ShapeType.ToInt();
        _sliderMax.MinValue = _dataGame.MinDigit;
        _sliderMax.Value = _dataGame.MaxDigit;

        SetStartInteractable(_dataGame.ModeStart == GameModeStart.GameContinue);
        _toggleContinue.OnValueChanged.AddListener(SetInvertInteractable);

        #region Local Functions
        void SetStartInteractable(bool value)
        {
            _toggleContinue.Interactable = value;
            _toggleContinue.IsOn = value;
            SetInvertInteractable(value);
        }
        void SetInvertInteractable(bool value)
        {
            value = !value;

            _sliderSize.Interactable = value;
            _sliderMax.Interactable = value;
            _rewardAdPanel.Interactable = value;
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
                _dataGame.ShapeType = Mathf.RoundToInt(_sliderSize.Value).ToEnum<ShapeSize>();
                _dataGame.MaxDigit = Mathf.RoundToInt(_sliderMax.Value);

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
                    Message.BannerKey("BombsAdd");
                    await UniTask.Delay(1000, true);
                }
            }
            return result;
        }
    }
        
}
