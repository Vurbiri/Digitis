using UnityEngine;

public class PositionDependsSizeCamera : MonoBehaviour
{
    [SerializeField] private CameraSize _cameraSize;

    private void Start()
    {
        Transform thisTransform = transform;
        Vector3 position = thisTransform.position;
        Vector3 newPosition;

        _cameraSize.EventChangingOffsetSizeX += (offsetX) =>
        {
            newPosition = position;
            newPosition.x += offsetX;
            thisTransform.position = newPosition;
        };
    }
}
