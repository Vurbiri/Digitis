using System;
using UnityEngine;

public class GameMobileController : MonoBehaviour
{
    [SerializeField] private ButtonPress _buttonLeft;
    [SerializeField] private ButtonPress _buttonRight;
    [SerializeField] private ButtonHold _buttonDown;
    [SerializeField] private ButtonPress _buttonRotation;
    [SerializeField] private ButtonClick _buttonBomb;
    [SerializeField] private ButtonClick _buttonPause;

    public bool ControlEnable { get; set; } = false;
    
    public event Action EventLeftPress;
    public event Action EventRightPress;
    public event Action EventStartDown;
    public event Action EventEndDown;
    public event Action EventRotationPress;
    public event Action EventBombClick;
    public event Action EventPause;
    public event Action EventUnPause;

    private void Awake()
    {
        _buttonLeft.EventButtonPress += () => OnGameEvent(EventLeftPress);
        _buttonRight.EventButtonPress += () => OnGameEvent(EventRightPress);

        _buttonDown.EventButtonStartHold += () => OnGameEvent(EventStartDown);
        _buttonDown.EventButtonEndHold += () => OnGameEvent(EventEndDown);

        _buttonRotation.EventButtonPress += () => OnGameEvent(EventRotationPress);

        _buttonBomb.EventButtonClick += () => OnGameEvent(EventBombClick);

        _buttonPause.EventButtonClick += () => { if (ControlEnable) EventPause?.Invoke(); else EventUnPause?.Invoke(); };

        void OnGameEvent(Action action)
        {
            if (ControlEnable)
                action?.Invoke();
        }
    }
}
