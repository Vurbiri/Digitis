using System;
using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
    public event Action<PooledObject> EventDeactivate;
    protected Transform _thisTransform;

    protected void Activate() => gameObject.SetActive(true);

    public virtual void Initialize()
    {
        _thisTransform = transform;
        gameObject.SetActive(false);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
        EventDeactivate?.Invoke(this);
    }

    public void SetParent(Transform parent)
    {
        _thisTransform.SetParent(parent);
    }
}
