using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class CanvasWorldDependsCamera : MonoBehaviour
{
    [SerializeField] private CameraReSize _cameraSize;

    private void Start()
    {
        RectTransform thisRectTransform = GetComponent<RectTransform>();
        Vector3 position = _cameraSize.transform.position;
        position.z = 0;
        thisRectTransform.position = position;

        _cameraSize.EventReSize += (halfSize) => thisRectTransform.sizeDelta = halfSize * 2f;
    }
}
