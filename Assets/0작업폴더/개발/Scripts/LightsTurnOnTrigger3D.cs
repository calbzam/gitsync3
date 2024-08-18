using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsTurnOnTrigger3D : MonoBehaviour
{
    [SerializeField] private Light[] _lightsToTurnOn;
    [SerializeField] private float _switchSpeed = 5;

    private int _lightsCount;
    private bool _turnOnStarted;
    private bool[] _isTurningOnLight;
    private float[] _onIntensity;

    private void Start()
    {
        _turnOnStarted = false;
        _lightsCount = _lightsToTurnOn.Length;

        _isTurningOnLight = new bool[_lightsCount];
        _onIntensity = new float[_lightsCount];

        for (int i = 0; i < _lightsCount; ++i)
        {
            _isTurningOnLight[i] = false;
            _onIntensity[i] = _lightsToTurnOn[i].intensity;
        }
    }

    private void Update()
    {
        if (_turnOnStarted) turnOnLights();
    }

    private void turnOnLights()
    {
        int cnt = 0;
        for (int i = 0; i < _lightsCount; ++i)
        {
            if (_isTurningOnLight[i])
            {
                _lightsToTurnOn[i].intensity = Mathf.MoveTowards(_lightsToTurnOn[i].intensity, _onIntensity[i], _switchSpeed * Time.deltaTime);

                if (_onIntensity[i] - _lightsToTurnOn[i].intensity < 0.01f)
                {
                    _lightsToTurnOn[i].intensity = _onIntensity[i];
                    _isTurningOnLight[i] = false;
                }
            }
            else ++cnt;
        }

        if (cnt == _lightsCount) _turnOnStarted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera Collider"))
        {
            _turnOnStarted = true;
            for (int i = 0; i < _lightsCount; ++i) { _isTurningOnLight[i] = true; }
        }
    }
}
