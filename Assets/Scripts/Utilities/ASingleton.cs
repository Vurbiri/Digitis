using UnityEngine;

public abstract class ASingleton<T> : MonoBehaviour where T : ASingleton<T>
{
    private static T _instance;
    private static bool _isQuit = false;

    [SerializeField] protected bool _isNotDestroying = true;
   
    public static T InstanceF => GetInstance(false);
    public static T InstanceFC => GetInstance(true);
    public static T Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance == null)
            _instance = this as T;
        else if (_instance != this)
            Destroy(gameObject);

        if (_isNotDestroying)
            DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    protected virtual void OnApplicationQuit() => _isQuit = true;

    private static T GetInstance(bool isCreate)
    {
        if (_instance == null && !_isQuit)
        {
            T[] instances = FindObjectsOfType<T>();
            int instancesCount = instances.Length;

            if (instancesCount > 0)
            {
                _instance = instances[0];
                for (int i = 1; i < instancesCount; i++)
                    Destroy(instances[i]);
            }
            else if(isCreate)
            {
                _instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }
        }

        return _instance;
    }
}
