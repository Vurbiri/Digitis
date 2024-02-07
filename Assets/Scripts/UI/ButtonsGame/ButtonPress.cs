using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
#if !UNITY_EDITOR
    , IPointerEnterHandler, IPointerExitHandler
#endif
{
    private WaitForSeconds _delay;
    private Coroutine _coroutine;

    public event Action EventButtonPress;

    private void Start()
    {
        Settings settings = Settings.InstanceF;
        SetSensitivity(settings.SensitivityButtons);
        settings.EventChangeSensitivityButtons += SetSensitivity;
    }

    private void StopPress()
    {
        if (_coroutine == null)
            return;
            
        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private void StartPress()
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

    private void SetSensitivity(float sensitivity) => _delay = new(sensitivity);

    public void OnPointerDown(PointerEventData eventData)
    {
        StartPress();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartPress();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopPress();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StopPress();
    }

    private void OnDestroy()
    {
        if(Settings.Instance != null)
            Settings.Instance.EventChangeSensitivityButtons -= SetSensitivity;
    }
}
