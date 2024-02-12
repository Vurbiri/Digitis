using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ASliderSetting : MonoBehaviour
{
    protected Slider _thisSlider;
    protected SettingsGame _settings;

    protected virtual void Awake()
    {
        _thisSlider = GetComponent<Slider>();
        _settings = SettingsGame.InstanceF;
    }
}
