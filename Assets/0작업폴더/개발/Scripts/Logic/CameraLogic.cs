using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _defaultVirtualCam;
    [SerializeField] private CinemachineVirtualCamera _freeVirtualCam;
    [SerializeField] private TMP_Text _toPlayerCamText;
    [SerializeField] private TMP_Text _toFreeCamText;

    private bool _isFreeCam = false;
    private float _freeCamPosZ;

    private void Start()
    {
        _defaultVirtualCam.gameObject.SetActive(true);
        _freeCamPosZ = _freeVirtualCam.GetComponent<FreeCameraDrag>().CamPosZ;

        SetFreeCam(_isFreeCam = false);
    }

    public void ToggleFreeCam()
    {
        SetFreeCam(_isFreeCam = !_isFreeCam);
    }

    private void SetFreeCam(bool freeCam)
    {
        if (freeCam) _freeVirtualCam.transform.position = new Vector3(_defaultVirtualCam.transform.position.x, _defaultVirtualCam.transform.position.y, _freeCamPosZ);

        _freeVirtualCam.gameObject.SetActive(freeCam);
        _toPlayerCamText.gameObject.SetActive(freeCam);
        _toFreeCamText.gameObject.SetActive(!freeCam);
    }
}
