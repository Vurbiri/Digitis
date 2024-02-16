using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private float _ratioPress = 1f;
    
    private WaitForSeconds _delay;
    private Coroutine _coroutine;

    public event Action EventButtonPress;

    private void Start()
    {
        SettingsGame settings = SettingsGame.InstanceF;
        SetSensitivity(settings.SensitivityButtons);
        settings.EventChangeSensitivityButtons += SetSensitivity;
    }

    private void SetSensitivity(float sensitivity) => _delay = new(sensitivity * _ratioPress);

    public void OnPointerDown(PointerEventData eventData)
    {
        _coroutine ??= StartCoroutine(StartPressCoroutine());

        IEnumerator StartPressCoroutine()
        {
            while (true)
            {
                EventButtonPress?.Invoke();
                yield return _delay;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopPress();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StopPress();
    }

    private void StopPress()
    {
        if (_coroutine == null)
            return;

        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private void OnDestroy()
    {
        if(SettingsGame.Instance != null)
            SettingsGame.Instance.EventChangeSensitivityButtons -= SetSensitivity;
    }
}
