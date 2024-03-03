using UnityEngine;
using UnityEngine.UI;

public class LevelBoard : ABoard
{
    [SerializeField] private Slider _shapesSlider;

    public Slider ShapesSlider => _shapesSlider;
    
    private void Start()
    {
        if (DataGame.Instance.IsInfinityMode)
        {
            gameObject.SetActive(false);
            return;
        }
        
        SetText(_dataGame.Level.ToString());
        _dataGame.EventChangeLevel += SetValue;

        SetMaxShapes(_dataGame.CountShapesMax);
        SetShapes(_dataGame.CountShapes);
        _dataGame.EventChangeCountShapes += SetShapes;
        _dataGame.EventChangeCountShapesMax += SetMaxShapes;
    }

    private void SetShapes(int count) => _shapesSlider.value = count;
    private void SetMaxShapes(int count)
    {
        _shapesSlider.maxValue = count;
        SetShapes(count);
    }

    private void OnDestroy()
    {
        if (DataGame.Instance != null && !DataGame.Instance.IsInfinityMode)
        {
            _dataGame.EventChangeScore -= SetText;
            _dataGame.EventChangeCountShapes -= SetShapes;
            _dataGame.EventChangeCountShapesMax -= SetMaxShapes;
        }
    }
}
