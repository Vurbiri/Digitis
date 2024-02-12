using UnityEngine;
using UnityEngine.UI;

public class LevelBoard : ABoard
{
    [SerializeField] private Slider _shapesSlider;
    
    private void Start()
    {
        SetValue(_dataGame.Level.ToString());
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
        if (DataGame.Instance != null)
        {
            _dataGame.EventChangeScore -= SetValue;
            _dataGame.EventChangeCountShapes -= SetShapes;
            _dataGame.EventChangeCountShapesMax -= SetMaxShapes;
        }
    }
}
