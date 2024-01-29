using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game : MonoBehaviour
{
    [SerializeField] private BlocksArea _area;
    [SerializeField] private ShapesManager _shapesManager;
    [SerializeField, Range(3,9)] private int _maxDigit = 9;
    [SerializeField] private bool _isGravity = true;

    private Dictionary<int, Vector2Int> _columns;

    private int _points = 0;


    private void Start()
    {
        _shapesManager.Initialize(_maxDigit, ShapeSize.Tromino);
        _shapesManager.EventEndMoveDown += OnBlockEndMoveDown;
        _shapesManager.CreateShape();

        _area.EventAddPoints += OnAddPoints;

        OnBlockEndMoveDown();
        StartCoroutine(Rotate());
        StartCoroutine(Shift());
    }

    private void OnAddPoints(int points)
    {
        _points += points;
        Debug.Log(_points);
    }

    private void OnBlockEndMoveDown()
    {
        if (FallColumns())
            return;

        OnBlockEndMoveDownAsync().Forget();

        async UniTaskVoid OnBlockEndMoveDownAsync()
        {
            _columns = await _area.CheckSeriesBlocksAsync();
            if (FallColumns())
                return;

            if (!_shapesManager.StartMove(_isGravity))
            {
                _shapesManager.EventEndMoveDown -= OnBlockEndMoveDown;
                StopCoroutine(Rotate());
                StopCoroutine(Shift());
                Debug.Log("Stop");
            }
        }

        bool FallColumns()
        {
            if (_columns == null || _columns.Count == 0)
                return false;

            List<Block> blocks;
            int x = 0,y = 0, count = 0;

            do
            {
                foreach(var (key, value) in _columns)
                {
                    x = key;
                    y = value.y;
                    count = value.x;
                    break;
                }
                _columns.Remove(x);
                blocks = _area.GetBlocksInColumn(x,y);
            }
            while (blocks.Count == 0 && _columns.Count > 0);

            if(blocks.Count == 0)
                return false;

            _shapesManager.StartFall(blocks, _isGravity, count);
            return true;
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
            _shapesManager.Shift(Random.value > 0.5 ? Direction2D.Left : Direction2D.Right);
            yield return new WaitForSeconds(0.13f);
        }
    }

}
