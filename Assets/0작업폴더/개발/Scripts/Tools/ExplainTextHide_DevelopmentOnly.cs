using UnityEngine;
using TMPro;

public class ExplainTextHide_DevelopmentOnly : MonoBehaviour
{
    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        ExplainTextHideButton.ExplainTextEnabled += ExplainTextToggleAction;
    }

    private void OnDisable()
    {
        ExplainTextHideButton.ExplainTextEnabled -= ExplainTextToggleAction;
    }

    private void ExplainTextToggleAction(bool enabled)
    {
        _text.enabled = enabled;
    }
}
