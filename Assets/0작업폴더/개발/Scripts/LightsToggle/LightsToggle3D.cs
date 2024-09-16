using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsToggle3D : MonoBehaviour
{
    [SerializeField] private BoxCollider _triggerColToUse;
    private ColBounds3D _triggerColBounds;
    [SerializeField] private Light[] _lightsToToggle;
    [SerializeField] private float _switchSpeed = 5;
    
    private Collider _mainCameraCol;
    private bool _camIsInTrigger;

    private int _lightsCount;
    private bool _toggleStarted;
    private bool[] _toggleInProcess;
    private float[] _toIntensity, _onIntensity, _offIntensity;

    private bool setCamIsInTrigger()
    {
        return _camIsInTrigger = _triggerColBounds.OtherIsInSelf(_mainCameraCol);
    }

    private void Start()
    {
        _triggerColBounds = new ColBounds3D(_triggerColToUse);
        _mainCameraCol = Camera.main.GetComponentInChildren<Collider>();

        _toggleStarted = false;
        _lightsCount = _lightsToToggle.Length;

        _toggleInProcess = new bool[_lightsCount];
        _toIntensity = new float[_lightsCount];
        _onIntensity = new float[_lightsCount];
        _offIntensity = new float[_lightsCount];

        SetPresetIntensities();
        EvalLightsInitialStates();
    }

    private void SetPresetIntensities()
    {
        for (int i = 0; i < _lightsCount; ++i)
        {
            _toggleInProcess[i] = false;
            _onIntensity[i] = _lightsToToggle[i].intensity;
            _offIntensity[i] = 0;
        }
    }

    private void EvalLightsInitialStates()
    {
        if (setCamIsInTrigger())
        {
            for (int i = 0; i < _lightsCount; ++i) _lightsToToggle[i].intensity = _onIntensity[i];
        }
        else
        {
            for (int i = 0; i < _lightsCount; ++i) _lightsToToggle[i].intensity = _offIntensity[i];
        }
    }

    private void Update()
    {
        if (!_camIsInTrigger)
        {
            if (setCamIsInTrigger())
            {
                _toggleStarted = true;
                for (int i = 0; i < _lightsCount; ++i) { _toggleInProcess[i] = true; _toIntensity[i] = _onIntensity[i]; }
            }
        }
        else // _camIsInTrigger
        {
            if (!setCamIsInTrigger())
            {
                _toggleStarted = true;
                for (int i = 0; i < _lightsCount; ++i) { _toggleInProcess[i] = true; _toIntensity[i] = _offIntensity[i]; }
            }
        }

        if (_toggleStarted) toggleLights();
    }

    private void toggleLights()
    {
        int cnt = 0;
        for (int i = 0; i < _lightsCount; ++i)
        {
            if (_toggleInProcess[i])
            {
                _lightsToToggle[i].intensity = Mathf.MoveTowards(_lightsToToggle[i].intensity, _toIntensity[i], _switchSpeed * Time.deltaTime);
                if (Mathf.Abs(_lightsToToggle[i].intensity - _toIntensity[i]) < 0.001f)
                {
                    _lightsToToggle[i].intensity = _toIntensity[i];
                    _toggleInProcess[i] = false;
                }
            }
            else ++cnt;
        }

        if (cnt == _lightsCount) _toggleStarted = false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("MainCamera Collider"))
    //    {
    //        _toggleStarted = true;
    //        for (int i = 0; i < _lightsCount; ++i) { _toggleInProcess[i] = true; _toIntensity[i] = _onIntensity[i]; }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("MainCamera Collider"))
    //    {
    //        _toggleStarted = true;
    //        for (int i = 0; i < _lightsCount; ++i) { _toggleInProcess[i] = true; _toIntensity[i] = _offIntensity[i]; }
    //    }
    //}
}
