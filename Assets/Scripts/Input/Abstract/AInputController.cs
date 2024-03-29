using System;
using UnityEngine;

public abstract class AInputController : MonoBehaviour
{
    [SerializeField] protected ButtonPress _buttonLeft;
    [SerializeField] protected ButtonPress _buttonRight;
    [SerializeField] protected ButtonHold _buttonDown;
    [SerializeField] protected ButtonPress _buttonRotation;
    [SerializeField] protected ButtonClick _buttonBomb;
    [SerializeField] protected ButtonClick _buttonPause;
    [Space]
    [SerializeField] private ButtonUnPause[] _buttonsUnPause;

    public event Action EventLeftPress;
    public event Action EventRightPress;
    public event Action EventStartDown;
    public event Action EventEndDown;
    public event Action EventRotationPress;
    public event Action EventBombClick;
    public event Action EventPause;
    public event Action EventUnPause;

    public virtual bool ControlEnable { get; set; } = false;
    
    protected virtual void Awake()
    {
        ControlEnable = false;

        _buttonLeft.EventButtonPress += () => OnGameEvent(EventLeftPress);
        _buttonRight.EventButtonPress += () => OnGameEvent(EventRightPress);
        _buttonDown.EventButtonStartHold += () => OnGameEvent(EventStartDown);
        _buttonDown.EventButtonEndHold += () => OnGameEvent(EventEndDown);
        _buttonRotation.EventButtonPress += () => OnGameEvent(EventRotationPress);
        _buttonBomb.EventButtonClick += () => OnGameEvent(EventBombClick);
        _buttonPause.EventButtonClick += () => { if (ControlEnable) EventPause?.Invoke(); };

        foreach (var button in _buttonsUnPause)
            button.EventUnPause += () => { if (!ControlEnable) EventUnPause?.Invoke(); };

        void OnGameEvent(Action action)
        {
            if (ControlEnable)
                action?.Invoke();
        }
    }
}
