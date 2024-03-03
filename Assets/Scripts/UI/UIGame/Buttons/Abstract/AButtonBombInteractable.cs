using UnityEngine;

[RequireComponent(typeof(ButtonClick))]
public abstract class AButtonBombInteractable : MonoBehaviour
{
    protected ButtonClick _thisButtonClick;
    protected DataGame _dataGame;

    protected virtual void Awake()
    {
        _thisButtonClick = GetComponent<ButtonClick>();

        _dataGame = DataGame.InstanceF;
        SetStatus(_dataGame.CountBombs);
        _dataGame.EventChangeCountBombs += SetStatus;
    }

    protected abstract void SetStatus(int countBomb);

    protected virtual void OnDestroy()
    {
        if (DataGame.Instance != null)
            _dataGame.EventChangeCountBombs -= SetStatus;
    }
}
