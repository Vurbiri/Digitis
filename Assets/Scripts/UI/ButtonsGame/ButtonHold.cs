using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
#if !UNITY_EDITOR
    , IPointerEnterHandler, IPointerExitHandler
#endif
{
    public event Action EventButtonStartHold;
    public event Action EventButtonEndHold;

    public void OnPointerDown(PointerEventData eventData)
    {
        EventButtonStartHold?.Invoke();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventButtonStartHold?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EventButtonEndHold?.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        EventButtonEndHold?.Invoke();
    }
}
