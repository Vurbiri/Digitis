using UnityEngine;

public class RandomObjects<T> where T : class, IRandomizeObject
{
    public T Next
    {
        get
        {
            if (_isOne)
                return _randomizeObjects[0].RandomObject;

            Shuffle();

            int randomValue = RandomValue;
            T obj = null;
            _weightMax = 0;
            
            foreach (var rObj in _randomizeObjects)
            {
                if (rObj.CheckAndChangeWeight(randomValue, _weightMax))
                    obj = rObj.RandomObject;
                _weightMax = rObj.WeightMax;
            }
            return obj;
        }
    }

    private int RandomValue  =>  Random.Range(0, _weightMax); 

    private readonly RandomizeObject[] _randomizeObjects;
    private int _weightMax = 0;
    private readonly bool _isOne;

    public RandomObjects(T[] objects) : this (objects, objects.Length) { }

    public RandomObjects(T[] objects, int max)
    {
        _randomizeObjects = new RandomizeObject[max];
        _isOne = max == 1;

        for (int i = 0; i < max; i++)
        {
            _randomizeObjects[i] = new(objects[i], _weightMax, max);
            _weightMax = _randomizeObjects[i].WeightMax;
        }
    }

    public T[] NextRange(int countObjects)
    {
        T[] objects = new T[countObjects];
        T obj;
        int count;
        bool isLimit = false;
        for (int i = 0; i < countObjects; i++)
        {
            do
            {
                obj = NextValue();
                count = obj.MaxCount;

                for (int j = 0; j < i; j++)
                {
                    if (obj == objects[j])
                        count--;
                    isLimit = count <= 0;
                    if (isLimit) break;
                }

            } while (isLimit);

            objects[i] = obj;
        }
        return objects;

        T NextValue()
        {
            int randomValue = RandomValue;
            foreach (var rObj in _randomizeObjects)
            {
                if (rObj.Check(randomValue))
                    return rObj.RandomObject;
            }
            return null;
        }
    }

    private void Shuffle()
    {
        RandomizeObject temp;
        for (int i = _randomizeObjects.Length - 1, j; i >= 1; i--)
        {
            j = Random.Range(0, i + 1);
            temp = _randomizeObjects[j];
            _randomizeObjects[j] = _randomizeObjects[i];
            _randomizeObjects[i] = temp;
        }
    }

    private class RandomizeObject
    {
        public T RandomObject { get; }
        public int WeightMax => _weightMin + _weightCurrent;

        public int _weightMin;
        public int _weightCurrent;

        private readonly int _incrementWeight;
        private readonly int _decrementWeight;
        private readonly int _capWeight;

        public RandomizeObject(T obj, int weightMin, int maxCount)
        {
            RandomObject = obj;

            if (maxCount == 1) return;

            _weightMin = weightMin;
            _weightCurrent = obj.Weight;

            _incrementWeight = Mathf.CeilToInt(_weightCurrent  / maxCount);
            _decrementWeight = _incrementWeight * (maxCount - 1);
            _capWeight = _weightCurrent + _decrementWeight;
        }

        public bool Check(int randomValue)
        {
            return randomValue >= _weightMin && randomValue < WeightMax;
        }

        public bool CheckAndChangeWeight(int randomValue, int sumWeight)
        {
            if (Check(randomValue))
            {
                _weightCurrent = Clamp(_weightCurrent - _decrementWeight);
                _weightMin = sumWeight;
                return true;
            }
            _weightCurrent = Clamp(_weightCurrent + _incrementWeight);
            _weightMin = sumWeight;
            return false;

            int Clamp(int value) => Mathf.Clamp(value, _incrementWeight, _capWeight);
        }
    }
}
