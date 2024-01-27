using System.Collections;
using UnityEngine;


public class GameLogic : MonoBehaviour
{
    [SerializeField] private Vector2Int _sizeArea = new(10, 20);
    [Space]
    [SerializeField] private PoolBlockData _poolBlockData;
    [Space]
    [SerializeField] private Speeds _speeds;

    private BlocksArea _area;
    private AShapes _shapes;

    private int _maxDigit = 9;

    Coroutine _coroutine;

    private void Awake()
    {
        _area = new(_sizeArea);
        _shapes = new Domino(_poolBlockData, _speeds, _area, _maxDigit);
        _shapes.EventEndMoveDown += OnBlockEndMoveDown;
        _shapes.CreateForm();
    }

    private void Start()
    {
        OnBlockEndMoveDown();
        _coroutine = StartCoroutine(Rotate());
    }



    private void OnBlockEndMoveDown()
    {
        if (!_shapes.StartMove())
        {
            _shapes.EventEndMoveDown -= OnBlockEndMoveDown;
            StopCoroutine(Rotate());
        }
    }

    private IEnumerator Rotate()
    {
        while(true) 
        {
            _shapes.Rotate();
            yield return new WaitForSeconds(0.75f);
        }
    }

}
