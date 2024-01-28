using System.Collections;
using UnityEngine;


public class Game : MonoBehaviour
{
    [SerializeField] private BlocksArea _area;
    [SerializeField] private ShapesManager _shapesManager;
    [SerializeField] private int _maxDigit = 9;


    private void Start()
    {
        _shapesManager.Initialize(_maxDigit, ShapeSize.Tromino);
        _shapesManager.EventEndMoveDown += OnBlockEndMoveDown;
        _shapesManager.CreateForm();

        OnBlockEndMoveDown();
        StartCoroutine(Rotate());
        StartCoroutine(Shift());
    }



    private void OnBlockEndMoveDown()
    {
        if (!_shapesManager.StartMove())
        {
            _shapesManager.EventEndMoveDown -= OnBlockEndMoveDown;
            StopCoroutine(Rotate());
            StopCoroutine(Shift());
            Debug.Log("Stop");
        }
    }

    private IEnumerator Rotate()
    {
        while(true) 
        {
            yield return new WaitForSeconds(0.3f);
            _shapesManager.Rotate();
        }
    }

    private IEnumerator Shift()
    {
        while (true)
        {
            _shapesManager.Shift(Random.value > 0.5 ? Vector2Int.left : Vector2Int.right);
            yield return new WaitForSeconds(0.13f);
        }
    }

}
