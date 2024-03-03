using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardControlPanel : MonoBehaviour
{
    [SerializeField] private Image _imageControl;
    [SerializeField] private Image _imagePause;
    [SerializeField] private Image _imageBomb;
    [Space]
    [SerializeField] private KeySettings[] _keySettings;
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

        _keySettings[_index].Setup(_imageControl, _imagePause, _imageBomb);
    }

    private void OnEnable()
    {
        _imageControl.color = _colorOn;
        _imagePause.color = _colorOn;
        _imageBomb.color = _colorOn;
        StartCoroutine(SwitchControlCoroutine());

        IEnumerator SwitchControlCoroutine()
        {
            while (true)
            {
                yield return _timeView;

                if (++_index >= _keySettings.Length)
                    _index = 0;

                yield return StartCoroutine(FadeColorCoroutine(_colorOff));

                _keySettings[_index].Setup(_imageControl, _imagePause, _imageBomb);

                yield return StartCoroutine(FadeColorCoroutine(_colorOn));
            }
        }

        IEnumerator FadeColorCoroutine(Color color)
        {
            _imageControl.CrossFadeColor(color, _secondsSwitch, false, true);
            _imagePause.CrossFadeColor(color, _secondsSwitch, false, true);
            _imageBomb.CrossFadeColor(color, _secondsSwitch, false, true);
            yield return _timeSwitch;
        }
    }

    #region Nested Classe
    [System.Serializable]
    private class KeySettings
    {
        [SerializeField] private Sprite _spriteControl;
        [SerializeField] private Sprite _spritePause;
        [SerializeField] private bool _isSmallPause;
        [SerializeField] private Sprite _spriteBomb;
        [SerializeField] private bool _isSmallBomb;

        private readonly Vector2 _small = Vector2.one * 1.25f;
        private readonly Vector2 _big = Vector2.one * 1.5f;

        public void Setup(Image imageControl, Image imagePause, Image imageBomb)
        {
            imageControl.sprite = _spriteControl;
            imagePause.sprite = _spritePause;
            imagePause.rectTransform.sizeDelta = _isSmallPause ? _small : _big;
            imageBomb.sprite = _spriteBomb;
            imageBomb.rectTransform.sizeDelta = _isSmallBomb ? _small : _big;
        }
    }
    #endregion
}
