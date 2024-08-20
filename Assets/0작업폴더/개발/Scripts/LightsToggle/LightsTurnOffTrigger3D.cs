using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsTurnOffTrigger3D : MonoBehaviour
{
    [SerializeField] private Light[] _lightsToTurnOff;
    [SerializeField] private float _switchSpeed = 5;

    private int _lightsCount;
    private bool _turnOffStarted;
    private bool[] _isTurningOffLight;
    private float[] _offIntensity;

    private void Start()
    {
        _turnOffStarted = false;
        _lightsCount = _lightsToTurnOff.Length;

        _isTurningOffLight = new bool[_lightsCount];
        _offIntensity = new float[_lightsCount];

        for (int i = 0; i < _lightsCount; ++i)
        {
            _isTurningOffLight[i] = false;
            _offIntensity[i] = 0;
        }
    }

    private void Update()
    {
        if (_turnOffStarted) turnOffLights();
    }

    private void turnOffLights()
    {
        int cnt = 0;
        for (int i = 0; i < _lightsCount; ++i)
        {
            if (_isTurningOffLight[i])
            {
                _lightsToTurnOff[i].intensity = Mathf.MoveTowards(_lightsToTurnOff[i].intensity, _offIntensity[i], _switchSpeed * Time.deltaTime);

                if (_lightsToTurnOff[i].intensity - _offIntensity[i] < 0.01f)
                {
                    _lightsToTurnOff[i].intensity = _offIntensity[i];
                    _isTurningOffLight[i] = false;
                }
            }
            else ++cnt;
        }

        if (cnt == _lightsCount) _turnOffStarted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera Collider"))
        {
            _turnOffStarted = true;
            for (int i = 0; i < _lightsCount; ++i) { _isTurningOffLight[i] = true; }
        }
    }
}
