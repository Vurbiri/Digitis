using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonalPanelAndScoreGame : MonoBehaviour
{
    [SerializeField] private PersonalPanel _personalPanel;
    [SerializeField] private ScoreBoard _scoreBoard;
    [Space]
    [SerializeField] private RawImage _avatar;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _textScore;

    private void Awake()
    {
        Localization.Instance.EventSwitchLanguage += SetLocalizationName;

        _name.text = _personalPanel.Name;
        _avatar.texture = _personalPanel.Avatar;
    }

    private void OnEnable()
    {
        _textScore.text = _scoreBoard.Value;
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
