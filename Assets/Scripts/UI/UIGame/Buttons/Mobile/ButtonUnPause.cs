using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonUnPause : MonoBehaviour
{
    protected Button _thisButton;
    
    public event Action EventUnPause;

    private void Awake()
    {
        (_thisButton = GetComponent<Button>()).onClick.AddListener(OnClick);
    }

    protected void OnClick() => EventUnPause?.Invoke();
}
