using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowHideToolButtons : MonoBehaviour
{
    [SerializeField] private TMP_Text _hideToolsText;
    [SerializeField] private TMP_Text _showToolsText;
    [SerializeField] private Button[] _toolButtonsToHide;

    private bool _toolsHidden = false;

    public void ToggleToolsHidden()
    {
        SetToolsHidden(_toolsHidden = !_toolsHidden);
    }

    private void Start()
    {
        SetToolsHidden(_toolsHidden = true); // hide test tools on startup
    }

    private void SetToolsHidden(bool hideTools)
    {
        foreach (Button button in _toolButtonsToHide)
        {
            button.gameObject.SetActive(!hideTools);
        }
        _showToolsText.gameObject.SetActive(hideTools);
        _hideToolsText.gameObject.SetActive(!hideTools);
    }
}
