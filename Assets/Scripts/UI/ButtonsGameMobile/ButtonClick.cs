using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClick : MonoBehaviour, IPointerDownHandler 
#if !UNITY_EDITOR
    , IPointerEnterHandler
#endif
{
    public event Action EventButtonClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        EventButtonClick?.Invoke();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventButtonClick?.Invoke();
    }
}
