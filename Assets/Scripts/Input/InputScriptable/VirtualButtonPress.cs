using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonPress", menuName = "Digitis/VirtualButton/Press", order = 51)]
public class VirtualButtonPress : ScriptableObject
{
    [SerializeField, InputAxis] private string _axe;
    [SerializeField] private bool _isPositive;
    [SerializeField] private float _ratioPress = 1f;

    private Action _event;
    private MonoBehaviour _mono;
    private Func<float, bool> _funcCompare;

    private WaitForSeconds _delay;
    private Coroutine _coroutine;

    public void Initialize(Action eventAction, MonoBehaviour mono)
    {
        _event = eventAction;
        _mono = mono;

        _funcCompare = _isPositive? (f) => f > 0 : (f) => f < 0;

        SettingsGame settings = SettingsGame.InstanceF;
        SetSensitivity(settings.SensitivityButtons);
        settings.EventChangeSensitivityButtons += SetSensitivity;
    }

    public void Update()
    {
        if (_funcCompare(Input.GetAxis(_axe)))
            OnDown();
        else
            StopCoroutine();


        #region Local Functions
        void OnDown()
        {
            _coroutine ??= _mono.StartCoroutine(StartPressCoroutine());

            #region Local Functions
            IEnumerator StartPressCoroutine()
            {
                while (true)
                {
                    _event?.Invoke();
                    yield return _delay;
                }
            }
            #endregion
        }
        #endregion
    }

    private void StopCoroutine()
    {
        if (_coroutine == null)
            return;

        _mono.StopCoroutine(_coroutine);
        _coroutine = null;
    }

    public void Destroy()
    {
        if(_mono != null)
            StopCoroutine();
        if (SettingsGame.Instance != null)
            SettingsGame.Instance.EventChangeSensitivityButtons -= SetSensitivity;
    }

    private void SetSensitivity(float sensitivity) => _delay = new(sensitivity * _ratioPress);

}
