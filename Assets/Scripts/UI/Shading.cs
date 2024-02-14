using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Shading : MonoBehaviour
{
    [SerializeField] float _fadeDuration = 0.5f;

    private void Start()
    {
        Image image = GetComponent<Image>();
        Color targetColor = image.color;
        targetColor.a = 0f;

        image.CrossFadeColor(targetColor, _fadeDuration, true, true);
    }
}
