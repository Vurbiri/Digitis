using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class FPSGraph : MonoBehaviour
{
    [SerializeField] private int _width = 128;
    [SerializeField] private int _height = 64;
    [SerializeField] private FilterMode _filterMode = FilterMode.Bilinear;
    [Space]
    [SerializeField] private Color _colorBackground = Color.white;
    [SerializeField] private Color _colorGraph = Color.black;
    [SerializeField] private Color _colorAvg = Color.blue;

    private Texture2D _texture;
    private List<int> _pixels;

    private void Start()
    {
        _texture = new(_width, _height)
        {
            filterMode = _filterMode
        };

        GetComponent<RawImage>().texture = _texture;

        _pixels = new(_width);

    }

    public (float avg, int min, int max) UpdateTexture(int fps, int fpsMax)
    {
        if (_pixels.Count == _width)
            _pixels.RemoveAt(0);

        _pixels.Add(fps);
        int count = _pixels.Count;

        ClearTexture();

        float scale = _height / (fpsMax * 1.15f);
        float avg = 0f;
        int min = int.MaxValue;
        int max = int.MinValue;
        int value;
        int x, y;
        for (int i = 0; i < count; i++)
        {
            value = _pixels[i];
            x = _width - count + i;
            y = Mathf.RoundToInt(value * scale);
            _texture.SetPixel(x, y, _colorGraph);

            avg += value;
            min = Mathf.Min(value, min);
            max = Mathf.Max(value, max);
        }
        avg /= count;

        y = Mathf.RoundToInt(avg * scale);
        for (x = 0; x < _width; x++)
            _texture.SetPixel(x, y, _colorAvg);

        _texture.Apply();

        return (avg, min, max);
    }

    private void OnDisable()
    {
        _pixels?.Clear();
        ClearTexture();
        _texture.Apply();
    }

    private void ClearTexture()
    {
        for (int i = 0; i < _width; i++)
            for (int j = 0; j < _height; j++)
                _texture.SetPixel(i, j, _colorBackground);
    }
}
