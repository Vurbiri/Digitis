using UnityEngine;
using UnityEngine.UI;

public class BlockDigitUI : MonoBehaviour
{
    [SerializeField] private BlockParticleDigitUI _particle;
    [SerializeField] private Image _block;
    [SerializeField] private Image _digit;

    public void SetupBlock(BlockSettings settings, Material material)
    {
        _block.sprite = settings.SpriteBlock;
        _block.color = settings.ColorBlock;
        _block.enabled = false;

        _digit.sprite = settings.SpriteNumber;
        _digit.color = settings.ColorNumber;
        _digit.enabled = false;

        _particle.SetupBlock(material, settings.ColorBlock);
    }

    public void ShowBlock()
    {
        _block.enabled = true;
        _digit.enabled = true;
    }

    public void StartParticle()
    {
        _block.enabled = false;
        _digit.enabled = false;

        StartCoroutine(_particle.Run());
    }

    public void Hide()
    {
        _block.enabled = false;
        _digit.enabled = false;

        StopCoroutine(_particle.Run());
        _particle.ClearAndStop();
    }
}
