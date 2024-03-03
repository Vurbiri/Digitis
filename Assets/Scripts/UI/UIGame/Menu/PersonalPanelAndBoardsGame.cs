using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonalPanelAndBoardsGame : MonoBehaviour
{
    [SerializeField] private PersonalPanel _personalPanel;
    [SerializeField] private LevelBoard _levelBoard;
    [SerializeField] private MaxScoreBoard _scoreMaxBoard;
    [SerializeField] private ScoreBoard _scoreBoard;
    [SerializeField] private BombBoard _bombBoard;
    [Space]
    [SerializeField] private GameObject _levelPanel;
    [SerializeField] private GameObject _scoreMaxPanel;
    [Space]
    [SerializeField] private RawImage _avatar;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _textLevel;
    [SerializeField] private TMP_Text _textMaxScore;
    [SerializeField] private TMP_Text _textScore;
    [SerializeField] private TMP_Text _textBomb;
    [SerializeField] private Slider _shapesSlider;

    private bool _isInfinityMode;

    private void Awake()
    {
        Localization.Instance.EventSwitchLanguage += SetLocalizationName;

        _name.text = _personalPanel.Name;
        _avatar.texture = _personalPanel.Avatar;

        _isInfinityMode = DataGame.Instance.IsInfinityMode;

        _levelPanel.SetActive(!_isInfinityMode);
        _scoreMaxPanel.SetActive(_isInfinityMode);
    }

    private void OnEnable()
    {
        _textScore.text = _scoreBoard.Value;
        _textBomb.text = _bombBoard.Value;

        if (_isInfinityMode)
        {
            _textMaxScore.text = _scoreMaxBoard.Value;
        }
        else
        {
            _textLevel.text = _levelBoard.Value;
            _shapesSlider.maxValue = _levelBoard.ShapesSlider.maxValue;
            _shapesSlider.value = _levelBoard.ShapesSlider.value;
        }
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
