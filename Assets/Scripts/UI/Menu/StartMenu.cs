using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MenuNavigation
{
    [Scene]
    [SerializeField] private int _sceneGame = 2;
    [Space]
    [SerializeField] private SelectableInteractable _continue;
    [SerializeField] private Toggle _toggleNew;
    [Space]
    [SerializeField] private SelectableInteractable _size;
    [SerializeField] private SelectableInteractable _max;

    private Slider _sliderSize;
    private Slider _sliderMax;

    private DataGame _data;

    private void Start()
    {
        _data = DataGame.InstanceF;

        Toggle toggleContinue = _continue.ThisSelectable as Toggle;
        _sliderSize = _size.ThisSelectable as Slider;
        _sliderMax = _max.ThisSelectable as Slider;

        _sliderSize.value = _data.ShapeType.ToInt();
        _sliderMax.value = _data.MaxDigit;

        _size.Interactable = _max.Interactable = !(toggleContinue.isOn = _continue.Interactable = _data.ModeStart == GameModeStart.GameContinue);
        toggleContinue.onValueChanged.AddListener((isOn) => _size.Interactable = _max.Interactable = !isOn);
        //_toggleNew.isOn = !_continue.Interactable;
    }

    public void OnStart()
    {
        if(_toggleNew.isOn)
        {
            _data.ResetData();
            _data.ShapeType = Mathf.RoundToInt(_sliderSize.value).ToEnum<ShapeSize>();
            _data.MaxDigit = Mathf.RoundToInt(_sliderMax.value);
        }

        SceneManager.LoadSceneAsync(_sceneGame);
    }
}
