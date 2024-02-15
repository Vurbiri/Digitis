using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class Version : MonoBehaviour
{
    void Start() => GetComponent<TMP_Text>().text = Application.version;
}
