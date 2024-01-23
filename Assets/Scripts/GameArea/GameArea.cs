#if UNITY_EDITOR
using NaughtyAttributes;
#endif

using UnityEngine;

public class GameArea : MonoBehaviour
{
    [SerializeField] private Block _block;


    [Button]
    private void Test()
    {
        _block.MoveTargetY(10, 1f);
    }
}
