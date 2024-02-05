using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Game : MonoBehaviour
{
    [SerializeField] private BlocksArea _area;
    [SerializeField] private ShapesManager _shapesManager;
    [Space]
    [SerializeField] private ShapeSize _shapeType = ShapeSize.Tromino;
    [SerializeField, Range(3,9)] private int _maxDigit = 9;
    [SerializeField] private bool _isGravity = true;
    [SerializeField] private bool _isTetris = false;

    private Dictionary<int, Vector2Int> _columns;
    private List<int> _lines;

    private int _points = 0;


    private void Start()
    {
        if (_isTetris)
        {
            _shapesManager.InitializeTetris(_shapeType);
            _shapesManager.EventEndMoveDown += OnBlockEndMoveDownTetris;
        }
        else
        {
            _shapesManager.InitializeDigitis(_maxDigit, _shapeType);
            _shapesManager.EventEndMoveDown += OnBlockEndMoveDownDigitis;
        }


        _shapesManager.CreateShape();

        _area.EventAddPoints += OnAddPoints;

        if (_isTetris)
            OnBlockEndMoveDownTetris();
        else
            OnBlockEndMoveDownDigitis();

        StartCoroutine(Rotate());
        StartCoroutine(Shift());
    }

    private void OnAddPoints(int points)
    {
        _points += points;
        Debug.Log(_points);
    }

    private void OnBlockEndMoveDownDigitis()
    {
        if (FallColumns())
            return;

        OnBlockEndMoveDownAsync().Forget();

        async UniTaskVoid OnBlockEndMoveDownAsync()
        {
            _columns = await _area.CheckNewBlocksDigitisAsync();
            if (FallColumns())
                return;

            if (!_shapesManager.StartMove(_isGravity))
            {
                _shapesManager.EventEndMoveDown -= OnBlockEndMoveDownDigitis;
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
            KeyValuePair<int, Vector2Int> element;

            do
            {
                element = _columns.First(); 
                 _columns.Remove(element.Key);
                blocks = _area.GetBlocksInColumn(element.Key, element.Value.y);
            }
            while (blocks.Count == 0 && _columns.Count > 0);

            if(blocks.Count == 0)
                return false;

            _shapesManager.StartFall(blocks, _isGravity, element.Value.x);
            return true;
        }
    }

    private void OnBlockEndMoveDownTetris()
    {
        if (FallColumns())
            return;

        OnBlockEndMoveDownAsync().Forget();

        async UniTaskVoid OnBlockEndMoveDownAsync()
        {
            _lines = await _area.CheckNewBlocksTetrisAsync();
            if (FallColumns())
                return;

            if (!_shapesManager.StartMove(false))
            {
                _shapesManager.EventEndMoveDown -= OnBlockEndMoveDownTetris;
                StopCoroutine(Rotate());
                StopCoroutine(Shift());
                Debug.Log("Stop");
            }
        }

        bool FallColumns()
        {
            if (_lines == null || _lines.Count == 0)
                return false;

            List<Block> blocks;
            int y;
            do
            {
                y = _lines[0];
                _lines.RemoveAt(0);
                blocks = _area.GetBlocksAboveLine(y);
            }
            while (blocks.Count == 0 && _lines.Count > 0);

            if (blocks.Count == 0)
                return false;

            _shapesManager.StartFall(blocks, false, 1);
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

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _shapesManager.ShapeToBomb();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _shapesManager.Shift(Direction2D.Left);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _shapesManager.Shift(Direction2D.Right);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _shapesManager.Rotate();
        }

    }

}
