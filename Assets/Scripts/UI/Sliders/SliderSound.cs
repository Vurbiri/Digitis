using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderSound : ASliderSetting
{
    [SerializeField] private MixerGroup _audioMixerGroup;
    [SerializeField] private TextLocalization _captionText;

    private void Start()
    {
        _thisSlider.minValue = _settings.MinValue;
        _thisSlider.maxValue = _settings.MaxValue;
        _thisSlider.onValueChanged.AddListener((v) => _settings.SetVolume(_audioMixerGroup, v));

        _captionText.Setup(_audioMixerGroup.ToString());
    }

    private void OnEnable() => _thisSlider.value = _settings.GetVolume(_audioMixerGroup);
}
