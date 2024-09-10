using System;
using UnityEngine;
using TMPro;

public class ExplainTextHideButton : MonoBehaviour
{
    public static event Action<bool> ExplainTextEnabled;

    private bool _explainTextEnabled;

    [SerializeField] private TMP_Text _toDisableText;
    [SerializeField] private TMP_Text _toEnableText;

    private void Start()
    {
        _explainTextEnabled = true;
        ShowButtonText(_explainTextEnabled);
    }

    public void ExplainTextToggle()
    {
        _explainTextEnabled = !_explainTextEnabled;
        ShowButtonText(_explainTextEnabled);
        ExplainTextEnabled?.Invoke(_explainTextEnabled);
    }

    private void ShowButtonText(bool enabled)
    {
        _toEnableText.gameObject.SetActive(!enabled);
        _toDisableText.gameObject.SetActive(enabled);
    }
}
