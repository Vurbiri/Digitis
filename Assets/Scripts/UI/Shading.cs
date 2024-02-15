using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Shading : MonoBehaviour
{
    [SerializeField] float _prePause = 0f;
    [SerializeField] float _fadeDuration = 0.5f;

    private IEnumerator Start()
    {
        Image image = GetComponent<Image>();
        Color targetColor = image.color;
        targetColor.a = 0f;

        yield return new WaitForSecondsRealtime(_prePause);
        image.CrossFadeColor(targetColor, _fadeDuration, true, true);
    }
}
