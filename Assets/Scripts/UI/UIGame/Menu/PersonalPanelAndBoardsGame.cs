using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonalPanelAndBoardsGame : MonoBehaviour
{
    [SerializeField] private PersonalPanel _personalPanel;
    [SerializeField] private LevelBoard _levelBoard;
    [SerializeField] private ScoreBoard _scoreBoard;
    [SerializeField] private BombBoard _bombBoard;
    [Space]
    [SerializeField] private RawImage _avatar;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _textLevel;
    [SerializeField] private TMP_Text _textScore;
    [SerializeField] private TMP_Text _textBomb;
    [SerializeField] private Slider _shapesSlider;

    private void Awake()
    {
        Localization.Instance.EventSwitchLanguage += SetLocalizationName;

        _name.text = _personalPanel.Name;
        _avatar.texture = _personalPanel.Avatar;
    }

    private void OnEnable()
    {
        _textLevel.text = _levelBoard.Value;
        _textScore.text = _scoreBoard.Value;
        _textBomb.text = _bombBoard.Value;

        _shapesSlider.maxValue = _levelBoard.ShapesSlider.maxValue;
        _shapesSlider.value = _levelBoard.ShapesSlider.value;
    }

    private void SetLocalizationName()
    {
        StartCoroutine(SetLocalizationNameCoroutine());

        IEnumerator SetLocalizationNameCoroutine()
        {
            yield return null;
            _name.text = _personalPanel.Name;
        }
    }

    private void OnDestroy()
    {
        if (Localization.Instance != null)
            Localization.Instance.EventSwitchLanguage -= SetLocalizationName;
    }
}
