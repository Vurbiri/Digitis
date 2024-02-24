using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    [SerializeField] private string _key = "Digitis";
    [Space]
    [SerializeField] private Letter _prefabLetter;
    [SerializeField] private RectTransform _container;
    [Space]
    [SerializeField] private float _size = 2;
    [SerializeField] private float _scaleContainer = 1.05f;
    [SerializeField] private bool _isX = true;
    [Space]
    [SerializeField] private BlockSettings[] _settingsBlocks;

    private readonly List<Letter> _title = new(7);

    public string Key { get => _key; set { _key = value; SetTitle(); } }

    private void Start()
    {
        SetTitle();
        Localization.Instance.EventSwitchLanguage += SetTitle;
    }

    private void SetTitle()
    {
        string title = Localization.Instance.GetText(_key).ToUpper();
        int count = title.Length;
        int childCount = _title.Count;

        Vector2 sizeContainer = _container.sizeDelta;
        if(_isX)
            sizeContainer.x = _size * count * _scaleContainer;
        else
            sizeContainer.y = _size * count * _scaleContainer;
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

    private void OnDestroy()
    {
        if (Localization.Instance != null)
            Localization.Instance.EventSwitchLanguage -= SetTitle;
    }
}
