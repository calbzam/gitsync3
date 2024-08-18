using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsToggleTrigger2D : MonoBehaviour
{
    [SerializeField] private Light[] _lightsToToggle;
    [SerializeField] private FromDirection _enableWhenEntered = FromDirection.FromBelow;
    [SerializeField] private FromDirection _disableWhenEntered = FromDirection.FromAbove;
    [SerializeField] private float _switchSpeed = 5;

    private int _lightsCount;
    private bool _toggleStarted;
    private bool[] _toggleInProcess;
    private float[] _toIntensity, _onIntensity, _offIntensity;

    private enum FromDirection
    {
        None,
        FromAbove,
        FromBelow,
        FromLeft,
        FromRight,
    }

    private void Start()
    {
        _toggleStarted = false;
        _lightsCount = _lightsToToggle.Length;

        _toggleInProcess = new bool[_lightsCount];
        _toIntensity = new float[_lightsCount];
        _onIntensity = new float[_lightsCount];
        _offIntensity = new float[_lightsCount];

        for (int i = 0; i < _lightsCount; ++i)
        {
            _toggleInProcess[i] = false;
            _onIntensity[i] = _lightsToToggle[i].intensity;
            _offIntensity[i] = 0;
        }

        evalCamPosition();
    }

    private void Update()
    {
        if (_toggleStarted) toggleLights();
    }

    private void evalCamPosition()
    {
        FromDirection evalDir = getFromDirection(Camera.main.GetComponentInChildren<Collider2D>()); // mainCameraCol

        if (evalDir == _disableWhenEntered)
        {
            for (int i = 0; i < _lightsCount; ++i) _lightsToToggle[i].intensity = _onIntensity[i];
        }
        else if (evalDir == _enableWhenEntered)
        {
            for (int i = 0; i < _lightsCount; ++i) _lightsToToggle[i].intensity = _offIntensity[i];
        }
    }

    private void toggleLights()
    {
        int cnt = 0;
        for (int i = 0; i < _lightsCount; ++i)
        {
            if (_toggleInProcess[i])
            {
                _lightsToToggle[i].intensity = Mathf.MoveTowards(_lightsToToggle[i].intensity, _toIntensity[i], _switchSpeed * Time.deltaTime);

                if (Mathf.Abs(_lightsToToggle[i].intensity - _toIntensity[i]) < 0.01f)
                {
                    _lightsToToggle[i].intensity = _toIntensity[i];
                    _toggleInProcess[i] = false;
                }
            }
            else ++cnt;
        }

        if (cnt == _lightsCount) _toggleStarted = false;
    }
    
    private FromDirection getFromDirection(Collider2D col)
    {
        if (col.transform.position.y > transform.position.y) return FromDirection.FromAbove;
        else if (col.transform.position.y < transform.position.y) return FromDirection.FromBelow;
        else if (col.transform.position.x < transform.position.x) return FromDirection.FromLeft;
        else if (col.transform.position.x > transform.position.x) return FromDirection.FromRight;
        else return FromDirection.None;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("MainCamera Collider"))
        {
            if (getFromDirection(col) == _enableWhenEntered)
            {
                _toggleStarted = true;
                for (int i = 0; i < _lightsCount; ++i) { _toggleInProcess[i] = true; _toIntensity[i] = _onIntensity[i]; }
            }
            else if (getFromDirection(col) == _disableWhenEntered)
            {
                _toggleStarted = true;
                for (int i = 0; i < _lightsCount; ++i) { _toggleInProcess[i] = true; _toIntensity[i] = _offIntensity[i]; }
            }
        }
    }
}
