using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBreak : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _parentRb;
    [SerializeField] private PanelWholepanelTimer _wholePanelTimer;
    [SerializeField] private GameObject _wholePanel;
    [SerializeField] private GameObject _leftHalf;
    [SerializeField] private GameObject _rightHalf;

    [Header("나무판넬이 부서지기까지 걸리는 시간 (초)")]
    public float DurationToBreak = 3f;

    private void Start()
    {
        SetPanelBroken(false);
    }

    private void Update()
    {
        MeasureTime();
    }

    private void SetPanelBroken(bool isBroken)
    {
        _parentRb.simulated = !isBroken;
        _leftHalf.SetActive(isBroken);
        _rightHalf.SetActive(isBroken);
        _wholePanel.SetActive(!isBroken);
    }

    private void MeasureTime()
    {
        if (_wholePanelTimer.TimerEnabled)
        {
            if (Time.time - _wholePanelTimer.StartTime > DurationToBreak)
            {
                _wholePanelTimer.TimerEnabled = false;
                SetPanelBroken(true);
            }
        }
    }
}
