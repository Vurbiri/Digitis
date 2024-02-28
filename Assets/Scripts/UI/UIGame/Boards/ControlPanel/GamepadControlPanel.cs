using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GamepadControlPanel : MonoBehaviour
{
    [SerializeField] private Image _imagePad;
    [Space]
    [SerializeField] private Sprite[] _spritesControl;
    [SerializeField] private int _index = 0;
    [Space]
    [SerializeField] private float _secondsView = 10f;
    [SerializeField] private float _secondsSwitch = 0.35f;

    private WaitForSeconds _timeView;
    private WaitForSeconds _timeSwitch;

    private Color _colorOn = Color.white;
    private Color _colorOff = Color.white;

    private void Awake()
    {
        _timeView = new(_secondsView);
        _timeSwitch = new(_secondsSwitch);

        _colorOff.a = 0;

        _imagePad.sprite = _spritesControl[_index];
    }

    private void OnEnable()
    {
        _imagePad.color = _colorOn;
        StartCoroutine(SwitchControlCoroutine());

        IEnumerator SwitchControlCoroutine()
        {
            while (true) 
            { 
                yield return _timeView;

                if(++_index >= _spritesControl.Length)
                    _index = 0;

                yield return StartCoroutine(FadeColorCoroutine(_colorOff)); 

                _imagePad.sprite = _spritesControl[_index];

                yield return StartCoroutine(FadeColorCoroutine(_colorOn));
            }
        }

        IEnumerator FadeColorCoroutine(Color color)
        {
            _imagePad.CrossFadeColor(color, _secondsSwitch, false, true);
            yield return _timeSwitch;
        }
    }
}
