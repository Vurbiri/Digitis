using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class SliderValue : MonoBehaviour
{
    [SerializeField] private Slider _parentSlider;

    private void Start()
    {
        TMP_Text thisText = GetComponent<TMP_Text>();
        string format = _parentSlider.wholeNumbers ? "N0" : "F";

        SetValue(_parentSlider.value);
        _parentSlider.onValueChanged.AddListener(SetValue);

        void SetValue(float value)
        {
            thisText.text = value.ToString(format);
        }
    }
}
