using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public event Action EventButtonStartHold;
    public event Action EventButtonEndHold;

    private bool _isHold = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isHold)
            return;
        _isHold = true;

        EventButtonStartHold?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HoldEnd();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HoldEnd();
    }


    private void HoldEnd()
    {
        if (!_isHold)
            return;
        _isHold = false;

        EventButtonEndHold?.Invoke();
    }
}
