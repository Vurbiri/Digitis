using System;
using UnityEngine;

public class GameMobileController : AGameController
{
    [SerializeField] private ButtonPress _buttonLeft;
    [SerializeField] private ButtonPress _buttonRight;
    [SerializeField] private ButtonHold _buttonDown;
    [SerializeField] private ButtonPress _buttonRotation;
    [SerializeField] private ButtonClick _buttonBomb;
    [SerializeField] private ButtonClick _buttonPause;


    private void Awake()
    {
        _buttonLeft.EventButtonPress += () => OnGameEvent(_eventLeftPress);
        _buttonRight.EventButtonPress += () => OnGameEvent(_eventRightPress);

        _buttonDown.EventButtonStartHold += () => OnGameEvent(_eventStartDown);
        _buttonDown.EventButtonEndHold += () => OnGameEvent(_eventEndDown);

        _buttonRotation.EventButtonPress += () => OnGameEvent(_eventRotationPress);

        _buttonBomb.EventButtonClick += () => OnGameEvent(_eventBombClick);

        _buttonPause.EventButtonClick += () => { if (ControlEnable) _eventPause?.Invoke(); else _eventUnPause?.Invoke(); };

        void OnGameEvent(Action action)
        {
            if (ControlEnable)
                action?.Invoke();
        }
    }
}
