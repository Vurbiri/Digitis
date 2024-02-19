using NaughtyAttributes;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonClick", menuName = "Digitis/VirtualButton/Click", order = 51)]
public class VirtualButtonClick : ScriptableObject
{
    [SerializeField, InputAxis] protected string _key;

    protected Action _event;

    public void Initialize(Action eventAction)
    {
        _event = eventAction;
    }

    public void Update()
    {
        if (Input.GetButtonDown(_key))
            _event?.Invoke();
    }
}
