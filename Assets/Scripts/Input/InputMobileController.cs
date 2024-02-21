using System;
using UnityEngine;

public class InputMobileController : AInputController
{
    [SerializeField] protected ButtonPress _buttonLeft;
    [SerializeField] protected ButtonPress _buttonRight;
    [SerializeField] protected ButtonHold _buttonDown;
    [SerializeField] protected ButtonPress _buttonRotation;
    [SerializeField] protected ButtonClick _buttonBomb;
    [SerializeField] protected ButtonClick _buttonPause;
    [SerializeField] protected PauseMenu _pauseMenu;


    protected virtual void Awake()
    {
        ControlEnable = false;

        _buttonLeft.EventButtonPress += () => OnGameEvent(_eventLeftPress);
        _buttonRight.EventButtonPress += () => OnGameEvent(_eventRightPress);

        _buttonDown.EventButtonStartHold += () => OnGameEvent(_eventStartDown);
        _buttonDown.EventButtonEndHold += () => OnGameEvent(_eventEndDown);

        _buttonRotation.EventButtonPress += () => OnGameEvent(_eventRotationPress);

        _buttonBomb.EventButtonClick += () => OnGameEvent(_eventBombClick);

        //_buttonPause.EventButtonClick += () => { if (ControlEnable) _eventPause?.Invoke(); };

        //_pauseMenu.EventClose += () => { if (!ControlEnable) _eventUnPause?.Invoke(); };

        void OnGameEvent(Action action)
        {
            if (ControlEnable)
                action?.Invoke();
        }
    }
}
