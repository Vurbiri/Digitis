using System;
using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
    public event Action<PooledObject> EventDeactivate;
    public Transform ThisTransform { get; private set; }

    protected virtual void Awake() =>
        ThisTransform = transform;
    

    public virtual void Activate() =>
        gameObject.SetActive(true);

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
        EventDeactivate?.Invoke(this);
    }
}
