using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelWholePanel : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _parentRb;
    [SerializeField] private GameObject _wholePanel;
    [SerializeField] private GameObject _leftHalf;
    [SerializeField] private GameObject _rightHalf;

    [Header("나무판넬이 부서지기까지 걸리는 시간 (초)")]
    public float DurationToBreak = 4f;

    private bool _timerEnabled;
    private float _startTime;

    private void Start()
    {
        _timerEnabled = false;
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
        if (_timerEnabled)
        {
            if (Time.time - _startTime > DurationToBreak)
            {
                _timerEnabled = false;
                SetPanelBroken(true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _timerEnabled = true;
            _startTime = Time.time;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _timerEnabled = false;
        }
    }
}
