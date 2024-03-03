using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClick : MonoBehaviour, IPointerDownHandler
{
    public event Action EventButtonClick;

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        EventButtonClick?.Invoke();
    }
}
