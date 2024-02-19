using System;
using UnityEngine;

public class GameDesktopController : AGameController
{
    [SerializeField] private VirtualButtonPress _buttonLeft;
    [SerializeField] private VirtualButtonPress _buttonRight;
    [SerializeField] private VirtualButtonHold _buttonDown;
    [SerializeField] private VirtualButtonPress _buttonRotation;
    [SerializeField] private VirtualButtonClick _buttonBomb;
    [SerializeField] private VirtualButtonClickTwoState _buttonPause;


    private void Awake()
    {
        ControlEnable = false;

        _buttonLeft.Initialize(_eventLeftPress, this);
        _buttonRight.Initialize(_eventRightPress, this);
        _buttonDown.Initialize(_eventStartDown, _eventEndDown);
        _buttonRotation.Initialize(_eventRotationPress, this);
        _buttonBomb.Initialize(_eventBombClick);
        _buttonPause.Initialize(_eventPause, _eventUnPause);
    }

    private void Update()
    {
        _buttonPause.UpdateParam(ControlEnable);

        if (!ControlEnable)
            return;

        _buttonLeft.Update();
        _buttonRight.Update();
        _buttonDown.Update();
        _buttonRotation.Update();
        _buttonBomb.Update();

    }

    private void OnDisable()
    {
        _buttonLeft.Destroy();
        _buttonRight.Destroy();
        _buttonRotation.Destroy();
    }
}
