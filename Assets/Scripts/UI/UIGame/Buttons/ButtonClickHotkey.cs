using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CustomTargetGraphic))]
public class ButtonClickHotkey : ButtonClick, IButtonInteractable, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [Space]
    [SerializeField, InputAxis] private string _key;
    [Space]
    [SerializeField] private bool _isActive = true;

    private bool _isInteractable;
    private CustomTargetGraphic _thisTargetGraphic;

    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;

            if (_thisTargetGraphic == null)
                return;

            if (IsInteractable)
                _thisTargetGraphic.SetNormalState();
            else
                _thisTargetGraphic.SetDisabledState();
        }
    }
    public bool IsInteractable 
    { 
        get => _isInteractable && _isActive;  
        set
        {
            if (_isInteractable == value) 
                return;
            
            _isInteractable = value;
            if(IsInteractable)
                _thisTargetGraphic.SetNormalState();
            else
                _thisTargetGraphic.SetDisabledState();
        }
    }

    public void Initialize(bool isInteractable)
    {
        _isInteractable = isInteractable;
        _thisTargetGraphic = GetComponent<CustomTargetGraphic>();
        _thisTargetGraphic.Initialize(isInteractable);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(!IsInteractable) return;

        _thisTargetGraphic.SetPressedState();

        base.OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!IsInteractable) return;

        _thisTargetGraphic.SetHighlightedState();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsInteractable) return;

        _thisTargetGraphic.SetHighlightedState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsInteractable) return;

        _thisTargetGraphic.SetNormalState();
    }

    private void Update()
    {
        if (!IsInteractable) return;

        if (Input.GetButtonDown(_key))
            base.OnPointerDown(null);
    }
}
