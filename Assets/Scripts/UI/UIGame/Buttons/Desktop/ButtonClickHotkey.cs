using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CustomTargetGraphic))]
public class ButtonClickHotkey : ButtonClick, IButtonInteractable, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [Space]
    [SerializeField, InputAxis] private string _key;

    protected bool _isInteractable;
    protected CustomTargetGraphic _thisTargetGraphic;
    private WaitForSecondsRealtime _timePress;

    public virtual bool IsInteractable 
    { 
        get => _isInteractable;  
        set
        {
            if (_isInteractable == value) 
                return;
            
            _isInteractable = value;
            if(value)
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

        _timePress = new(SettingsGame.Instance.SensitivityButtons);
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
        {
            OnPointerDown(null);
            StartCoroutine(OnPointerUPCoroutine());
        }

        #region Local Coroutine
        IEnumerator OnPointerUPCoroutine()
        {
            yield return _timePress;
            _thisTargetGraphic.SetNormalState();
        }
        #endregion
    }
}
