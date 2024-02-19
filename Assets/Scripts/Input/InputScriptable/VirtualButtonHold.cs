using NaughtyAttributes;
using System;
using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonHold", menuName = "Digitis/VirtualButton/Hold", order = 51)]
public class VirtualButtonHold : ScriptableObject
{
    [SerializeField, InputAxis] protected string _axe;
    [SerializeField] private bool _isPositive;

    private Func<float, bool> _funcCompare;
    private bool _isHold = false;

    private Action _eventStart;
    private Action _eventEnd;

    public void Initialize(Action eventActionStart, Action eventActionEnd)
    {
        _eventStart = eventActionStart;
        _eventEnd = eventActionEnd;

        _funcCompare = _isPositive ? (f) => f > 0 : (f) => f < 0;
    }

    public void Update()
    {
        if (_funcCompare(Input.GetAxis(_axe)))
            OnDown();
        else
            OnUp();

        #region Local Functions
        void OnDown()
        {
            if (_isHold)
                return;
            _isHold = true;

            _eventStart?.Invoke();
        }
        void OnUp()
        {
            if (!_isHold)
                return;
            _isHold = false;

            _eventEnd?.Invoke();
        }
        #endregion
    }
}
