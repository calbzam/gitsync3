using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _defaultVirtualCam;
    [SerializeField] private CinemachineVirtualCamera _freeVirtualCam;
    private FreeCamMove _freeCamMove;
    [SerializeField] private TMP_Text _toPlayerCamText;
    [SerializeField] private TMP_Text _toFreeCamText;

    private bool _isFreeCam = false;
    private float _freeCamPosZ;

    public void ToggleFreeCam_Button()
    {
        _isFreeCam = !_isFreeCam;
        if (!_isFreeCam) PlayerLogic.SetFreePlayerDrag(false); // swapped to Player Camera via button click
        SetFreeCam(_isFreeCam);
    }

    private void Start()
    {
        _defaultVirtualCam.gameObject.SetActive(true);
        _freeCamMove = _freeVirtualCam.GetComponent<FreeCamMove>();
        _freeCamPosZ = _freeCamMove.CamPosZ;

        //_freeCamMove.CamPosZUpdated += UpdateFreeCamPosZ; // Retain Z position of freeCam even when it is disabled then reenabled
        SetFreeCam(_isFreeCam = false);
    }

    //private void OnEnable()
    //{
    //    if (_freeCamMove) _freeCamMove.CamPosZUpdated += UpdateFreeCamPosZ;
    //}

    //private void OnDisable()
    //{
    //    _freeCamMove.CamPosZUpdated -= UpdateFreeCamPosZ;
    //}

    private void UpdateFreeCamPosZ(float newPosZ)
    {
        _freeCamPosZ = newPosZ;
    }

    public void SetFreeCam(bool freeCam)
    {
        _isFreeCam = freeCam;
        if (freeCam) _freeVirtualCam.transform.position = new Vector3(_defaultVirtualCam.transform.position.x, _defaultVirtualCam.transform.position.y, _freeCamPosZ);

        _freeVirtualCam.gameObject.SetActive(freeCam);
        _toPlayerCamText.gameObject.SetActive(freeCam);
        _toFreeCamText.gameObject.SetActive(!freeCam);
    }
}
