using UnityEngine;
using UnityEngine.UI;

public class LevelBoard : ABoard
{
    [SerializeField] private Slider _shapesSlider;
    
    private void Start()
    {
        SetValue(_game.Level.ToString());
        _game.EventChangeLevel += SetValue;

        SetMaxShapes(_game.CountShapesMax);
        SetShapes(_game.CountShapes);
        _game.EventChangeCountShapes += SetShapes;
        _game.EventChangeCountShapesMax += SetMaxShapes;

        void SetShapes(int count) => _shapesSlider.value = count;
        void SetMaxShapes(int count)
        {
            _shapesSlider.maxValue = count;
            SetShapes(count);
        }
    }
}
