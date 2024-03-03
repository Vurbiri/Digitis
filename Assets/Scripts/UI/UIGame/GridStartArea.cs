using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GridStartArea : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderBackground;
    [SerializeField] private Transform _transformSeparator;
    [SerializeField] private Transform _transformIcon;
    [Space]
    [SerializeField] private float _width = 10;
    [SerializeField] private float _startPosition = 10;
    [SerializeField] private float _startPositionIcon = -7;

    private const int SIZEE_FOR_TETROMIN0 = 3;
    private const int SIZEE_FOR_OTHER = 3;

    public int Initialize(bool isActive, ShapeSize sizeShape)
    {
        if (!isActive) 
        { 
            gameObject.SetActive(false);
            return 0;
        }

        int size = sizeShape == ShapeSize.Tetromino ? SIZEE_FOR_TETROMIN0 : SIZEE_FOR_OTHER;
        Vector2 sizeSprite = new(_width, size);

        GetComponent<SpriteRenderer>().size = sizeSprite;
        _renderBackground.size = sizeSprite;

        float offset = size / 2f;

        transform.localPosition = new(0f, _startPosition - offset, 0f);
        _transformSeparator.localPosition = new(0f, - offset, 0f);
        _transformIcon.localPosition = new(0f, _startPositionIcon - offset, 0f);

        return size;
    }
}
