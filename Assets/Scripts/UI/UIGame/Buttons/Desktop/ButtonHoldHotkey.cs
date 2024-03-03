using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CustomTargetGraphic))]
public class ButtonHoldHotkey : ButtonHold, IButtonInteractable, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [Space]
    [SerializeField, InputAxis] private string _axe;
    [SerializeField] private bool _isPositive;

    private Func<float, bool> _funcCompare;
    private CustomTargetGraphic _thisTargetGraphic;

    private bool _isHoldHotkey = false;
    protected bool _isInteractable = true;

    public bool IsInteractable
    {
        get => _isInteractable;
        set
        {
            if (_isInteractable == value)
                return;

            _isInteractable = value;
            if (value)
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

        _funcCompare = _isPositive ? (f) => f > 0 : (f) => f < 0;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!_isInteractable) return;

        _thisTargetGraphic.SetPressedState();
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!_isInteractable) return;

        if (_isHoldHotkey)
            _thisTargetGraphic.SetNormalState();
        else
            _thisTargetGraphic.SetHighlightedState();

        base.OnPointerUp(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isInteractable) return;

        _thisTargetGraphic.SetHighlightedState();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!_isInteractable) return;

        _thisTargetGraphic.SetNormalState();
        base.OnPointerExit(eventData);
    }

    private void Update()
    {
        if (!_isInteractable) return;

        if (_funcCompare(Input.GetAxis(_axe)))
        {
            OnPointerDown(null);
            _isHoldHotkey = true;
        }
        else if (_isHoldHotkey)
        {
            OnPointerUp(null);
            _isHoldHotkey = false;
        }
    }
}
