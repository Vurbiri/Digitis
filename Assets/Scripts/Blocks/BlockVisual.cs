using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlockVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRendererNumber;

    public void Setup(BlockSettings settings)
    {
        GetComponent<SpriteRenderer>().color = settings.ColorBlock;

        _spriteRendererNumber.sprite = settings.SpriteNumber;
        _spriteRendererNumber.color = settings.ColorNumber;
    }
}
