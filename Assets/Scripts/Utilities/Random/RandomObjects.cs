using UnityEngine;

public class RandomObjects<T> where T : class, IRandomizeObject
{
    public T Next
    {
        get
        {
            if (_isOne)
                return _randomizeObjects[0].RandomObject;

            int randomValue = RandomValue;
            T obj = null;
            _weightMax = 0;
            foreach (var rObj in _randomizeObjects)
            {
                if (rObj.CheckReduce(randomValue, _weightMax))
                    obj = rObj.RandomObject;
                _weightMax = rObj.WeightMax;
            }
            return obj;
        }
    }

    private int RandomValue => Random.Range(1, _weightMax);

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
            _randomizeObjects[i] = new(objects[i], _weightMax);
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


    private class RandomizeObject
    {
        public T RandomObject { get; }
        public int WeightMax => _weightMin + _weightCurrent;

        public int _weightMin;
        public int _weightCurrent;

        private const float REDUCE_WEIGHT = 0.5f;
        private const float INCREASE_WEIGHT = 1.1f;

        public RandomizeObject(T obj, int weightMin)
        {
            RandomObject = obj;
            _weightMin = weightMin;
            _weightCurrent = obj.Weight;
        }

        public bool Check(int randomValue)
        {
            return randomValue > _weightMin && randomValue <= WeightMax;
        }

        public bool CheckReduce(int randomValue, int sumWeight)
        {

            if (randomValue > _weightMin && randomValue <= WeightMax)
            {
                Debug.Log(_weightMin + " - " + randomValue + " - " + WeightMax);
                _weightCurrent = Mathf.RoundToInt(REDUCE_WEIGHT * _weightCurrent);
                _weightMin = sumWeight;
                return true;
            }
            _weightCurrent = Mathf.Clamp(Mathf.RoundToInt(INCREASE_WEIGHT * _weightCurrent), 0, RandomObject.Weight);
            _weightMin = sumWeight;
            return false;
        }
    }
}
