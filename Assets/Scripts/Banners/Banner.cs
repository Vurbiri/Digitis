using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Banner : PooledObject
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;
    [SerializeField] private Color[] _colors;

    [SerializeField] private int _fontSizeDesktop = 24;
    [SerializeField] private int _fontSizeMobile = 30;

    private Coroutine _coroutine;
    private Transform _oldParent;
    private bool _isThrough;

    private void Start()
    {
        _text.fontSize = SettingsGame.InstanceF.IsDesktop ? _fontSizeDesktop : _fontSizeMobile;
    }

    public void Setup(Transform newParent, string message, MessageType messageType, int time, bool isThrough)
    {
        _oldParent = ThisTransform.parent;
        _isThrough = isThrough;
        _text.text = message;
        _image.color = _colors[messageType.ToInt()];

        ThisTransform.SetParent(newParent);
        Activate();
        _coroutine = StartCoroutine(TimeShow());
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        
        IEnumerator TimeShow()
        {
            yield return new WaitForSecondsRealtime(time/1000f);
            Deactivate();
        }
    }

    public override void Deactivate()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        base.Deactivate();
        if(_oldParent != null)
            ThisTransform.SetParent(_oldParent);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (_isThrough)
            return;

        Deactivate();
    }


}
