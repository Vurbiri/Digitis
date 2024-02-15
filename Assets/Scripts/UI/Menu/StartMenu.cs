using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MenuNavigation
{
    [Scene]
    [SerializeField] private int _sceneGame = 2;
    [Space]
    [SerializeField] private LeaderboardUI _leaderboard;
    [Space]
    [SerializeField] private SelectableInteractable _continue;
    [Space]
    [SerializeField] private SelectableInteractable _size;
    [SerializeField] private SelectableInteractable _max;

    private Toggle _toggleContinue;

    private Slider _sliderSize;
    private Slider _sliderMax;

    private DataGame _data;

    private void Start()
    {
        _data = DataGame.InstanceF;

        _toggleContinue = _continue.ThisSelectable as Toggle;
        _sliderSize = _size.ThisSelectable as Slider;
        _sliderMax = _max.ThisSelectable as Slider;

        _sliderSize.value = _data.ShapeType.ToInt();
        _sliderMax.value = _data.MaxDigit;

        _size.Interactable = _max.Interactable = !(_toggleContinue.isOn = _continue.Interactable = _data.ModeStart == GameModeStart.GameContinue);
        _toggleContinue.onValueChanged.AddListener((isOn) => _size.Interactable = _max.Interactable = !isOn);
    }

    public void OnStart()
    {
        OnStartAsync().Forget();

        async UniTaskVoid OnStartAsync()
        {
            if (!_toggleContinue.isOn && _data.ModeStart == GameModeStart.GameContinue)
            {
                if (YandexSDK.Instance.IsLeaderboard)
                    await _leaderboard.TrySetScoreAndReward(_data.Score);
                _data.ResetData();
                _data.ShapeType = Mathf.RoundToInt(_sliderSize.value).ToEnum<ShapeSize>();
                _data.MaxDigit = Mathf.RoundToInt(_sliderMax.value);
            }

            await SceneManager.LoadSceneAsync(_sceneGame);
        }
    }
}
