using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuFirstSelect : MonoBehaviour
{
    [SerializeField] Selectable _firstSelected;
    GameObject _currentSelectedGameObject;

    protected virtual void OnEnable() => FirstSelect();

    protected virtual void Update()
    {
        _currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        if (_currentSelectedGameObject == null || !_currentSelectedGameObject.activeSelf)
            FirstSelect();
    }

    protected virtual void FirstSelect()
    {
        if (_firstSelected == null) return;

        //EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);
        _firstSelected.Select();
    }
}
