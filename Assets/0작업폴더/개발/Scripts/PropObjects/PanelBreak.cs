using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBreak : MonoBehaviour
{
    [Header("나무판넬이 부서지기까지 걸리는 시간 (초)")]
    public float DurationToBreak = 3f;

    [Header("")]
    [SerializeField] private Rigidbody2D _parentRb;
    [SerializeField] private PanelWholepanelTimer _wholePanelTimer;
    [SerializeField] private GameObject _wholePanel;
    [SerializeField] private GameObject _leftHalf;
    [SerializeField] private GameObject _rightHalf;

    private Vector3 _wholePanelPos, _leftHalfPos, _rightHalfPos;
    private Quaternion _wholePanelRot, _leftHalfRot, _rightHalfRot;

    private void OnEnable()
    {
        PlayerLogic.PlayerRespawned += PlayerRespawnedAction;
    }

    private void OnDisable()
    {
        PlayerLogic.PlayerRespawned -= PlayerRespawnedAction;
    }

    private void PlayerRespawnedAction()
    {
        ResetPanelTransforms();
    }

    private void Start()
    {
        SetPanelTransformVariables();
        SetPanelBroken(false);
    }

    private void Update()
    {
        MeasureTime();
    }

    private void SetPanelTransformVariables()
    {
        _wholePanelPos = _wholePanel.transform.position;
        _leftHalfPos = _leftHalf.transform.position;
        _rightHalfPos = _rightHalf.transform.position;

        _wholePanelRot = _wholePanel.transform.rotation;
        _leftHalfRot = _leftHalf.transform.rotation;
        _rightHalfRot = _rightHalf.transform.rotation;
    }

    private void ResetPanelTransforms()
    {
        SetPanelBroken(false);
        //_leftHalf.SetActive(true);
        //_rightHalf.SetActive(true);
        _wholePanel.transform.position = _wholePanelPos;
        _leftHalf.transform.position = _leftHalfPos;
        _rightHalf.transform.position = _rightHalfPos;

        _wholePanel.transform.rotation = _wholePanelRot;
        _leftHalf.transform.rotation = _leftHalfRot;
        _rightHalf.transform.rotation = _rightHalfRot;
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
