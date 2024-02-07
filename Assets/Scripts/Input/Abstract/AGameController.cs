using System;
using UnityEngine;

public abstract class AGameController : MonoBehaviour
{
    protected Action _eventLeftPress;
    protected Action _eventRightPress;
    protected Action _eventStartDown;
    protected Action _eventEndDown;
    protected Action _eventRotationPress;
    protected Action _eventBombClick;
    protected Action _eventPause;
    protected Action _eventUnPause;

    public bool ControlEnable { get; set; } = false;
    
    public event Action EventLeftPress { add { _eventLeftPress += value; }  remove { _eventLeftPress -= value; }  }
    public event Action EventRightPress { add { _eventRightPress += value; } remove { _eventRightPress -= value; } }
    public event Action EventStartDown { add { _eventStartDown += value; } remove { _eventStartDown -= value; } }
    public event Action EventEndDown { add { _eventEndDown += value; } remove { _eventEndDown -= value; } }
    public event Action EventRotationPress { add { _eventRotationPress += value; } remove { _eventRotationPress -= value; } }
    public event Action EventBombClick { add { _eventBombClick += value; } remove { _eventBombClick -= value; } }
    public event Action EventPause { add { _eventPause += value; } remove { _eventPause -= value; } }
    public event Action EventUnPause { add { _eventUnPause += value; } remove { _eventUnPause -= value; } }
}
