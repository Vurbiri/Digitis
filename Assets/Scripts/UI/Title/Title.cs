using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField] private Letter _prefabLetter;
    [SerializeField] private RectTransform _container;
    [Space]
    [SerializeField] private float _size = 2;
    [SerializeField] private float _scaleContainer = 1.05f;
    [Space]
    [SerializeField] private BlockSettings[] _settingsBlocks;

    private readonly List<Letter> _title = new(7);

    private const string KEY = "Digitis";

    private void Start()
    {
        SetTitle();
        Localization.Instance.EventSwitchLanguage += SetTitle;
    }

    private void SetTitle()
    {
        string title = Localization.Instance.GetText(KEY).ToUpper();
        int count = title.Length;
        int childCount = _title.Count;

        Vector2 sizeContainer = _container.sizeDelta;
        sizeContainer.x = _size * count * _scaleContainer;
        _container.sizeDelta = sizeContainer;

        HashSet<BlockSettings> settings = new(count);
        BlockSettings setting;
        Letter letter;
        for (int i = 0; i < count; i++)
        {
            do
            {
                setting = _settingsBlocks[Random.Range(0, _settingsBlocks.Length)];
            } 
            while (!settings.Add(setting));

            if (i < childCount)
            {
                letter = _title[i];
            }
            else
            {
                letter = Instantiate(_prefabLetter, _container);
                _title.Add(letter);
            }
            
            letter.Setup(setting, _size, title[i]);
        }

        for (int i = count; i < childCount; i++)
            _title[i].gameObject.SetActive(false);

    }
}
