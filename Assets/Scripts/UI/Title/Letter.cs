using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Letter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [Space]
    [SerializeField] private float _scaleText = 0.85f;
    
    private RectTransform _thisRectTransform;
    private Image _thisImage;

    private const string NAME = "Letter_";

    private void Awake()
    {
        _thisRectTransform = GetComponent<RectTransform>();
        _thisImage = GetComponent<Image>();
    }

    public void Setup(BlockSettings setting, float size, char letter)
    {
        _thisImage.color = setting.ColorBlock;
        _thisImage.sprite = setting.SpriteBlock;
        _thisRectTransform.sizeDelta = Vector2.one * size;

        _text.color = setting.ColorNumber;
        _text.text = new(letter, 1);
        _text.fontSize = size * _scaleText;

        gameObject.name = NAME + letter;
        gameObject.SetActive(true);
    }
}
