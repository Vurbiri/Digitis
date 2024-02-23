using NaughtyAttributes;
using UnityEngine;

public class ButtonUnPauseHotkey : ButtonUnPause
{
    [SerializeField, InputAxis] private string _key;

    private void Update()
    {
        if (!_thisButton.interactable) return;

        if (Input.GetButtonDown(_key))
            OnClick();
    }
}
