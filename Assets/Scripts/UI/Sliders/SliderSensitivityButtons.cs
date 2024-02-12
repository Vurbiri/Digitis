

public class SliderSensitivityButtons : ASliderSetting
{
    private void Start()
    {
        _thisSlider.onValueChanged.AddListener((v) => _settings.SensitivityButtons = v);
    }

    private void OnEnable() => _thisSlider.value = _settings.SensitivityButtons;
}
