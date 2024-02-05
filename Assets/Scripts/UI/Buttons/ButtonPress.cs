using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
#if !UNITY_EDITOR
    , IPointerEnterHandler, IPointerExitHandler
#endif
{
    [SerializeField] private float _ratePress = 0.5f;

    private WaitForSeconds _delay;
    private Coroutine _coroutine;

    public event Action EventButtonPress;

    private void Start()
    {
        _delay = new(_ratePress);
    }

    private void StopPress()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private void StartPress()
    {
        if (_coroutine == null)
            _coroutine = StartCoroutine(StartPressCoroutine());

        IEnumerator StartPressCoroutine()
        {
            while (true)
            {
                EventButtonPress?.Invoke();
                yield return _delay;
            }
        }
    }

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
}
