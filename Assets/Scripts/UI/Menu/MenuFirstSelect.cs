using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuFirstSelect : MonoBehaviour
{
    [SerializeField] Selectable _firstSelected;
    GameObject _currentSelectedGameObject;

    protected virtual void OnEnable() => FirstSelect();

    //protected virtual void Update()
    //{
    //        FirstSelect();
    //}

    protected virtual void FirstSelect()
    {
        if (_firstSelected == null) return;

        _currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        if (_currentSelectedGameObject == null || !_currentSelectedGameObject.activeSelf)
            _firstSelected.Select();

        //EventSystem.current.SetSelectedGameObject(_firstSelected.gameObject);

    }
}
