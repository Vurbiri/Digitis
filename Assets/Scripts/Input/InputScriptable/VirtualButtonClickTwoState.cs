using NaughtyAttributes;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonClick", menuName = "Digitis/VirtualButton/Click Two State", order = 51)]
public class VirtualButtonClickTwoState : ScriptableObject
{
    [SerializeField, InputAxis] protected string _key;

    protected Action _eventOne;
    protected Action _eventTwo;

    public void Initialize(Action eventActionOne, Action eventActionTwo)
    {
        _eventOne = eventActionOne;
        _eventTwo = eventActionTwo;
    }

    public void UpdateParam(bool isOne)
    {
        if (!Input.GetButtonDown(_key)) 
            return;

        if(isOne)
            _eventOne?.Invoke();
        else 
            _eventTwo?.Invoke();
    }
}
