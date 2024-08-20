using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FromDirection = ColBounds2D.FromDirection;

public class LightsToggleTrigger2D : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _triggerColSelf;
    [SerializeField] private Light[] _lightsToToggle;
    [SerializeField] private FromDirection _enableWhenEntered = FromDirection.FromBelow;
    [SerializeField] private FromDirection _disableWhenEntered = FromDirection.FromAbove;
    [SerializeField] private float _switchSpeed = 5;

    private ColBounds2D _triggerColBounds;
    private Collider2D _mainCameraCol;
    private int _lightsCount;
    private bool _toggleStarted;
    private bool[] _toggleInProcess;
    private float[] _toIntensity, _onIntensity, _offIntensity;

    private void Start()
    {
        _triggerColBounds = new ColBounds2D(_triggerColSelf);
        _mainCameraCol = Camera.main.GetComponentInChildren<Collider2D>();

        _lightsCount = _lightsToToggle.Length;
        _toggleStarted = false;

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

        evalLightsIntensity();
    }

    private void Update()
    {
        if (_toggleStarted) toggleLights();
    }
    
    private void evalLightsIntensity()
    {
        FromDirection evalDir = _triggerColBounds.GetRelativePosition(_mainCameraCol);

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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("MainCamera Collider"))
        {
            if (_triggerColBounds.GetRelativePosition(col) == _enableWhenEntered)
            {
                _toggleStarted = true;
                for (int i = 0; i < _lightsCount; ++i) { _toggleInProcess[i] = true; _toIntensity[i] = _onIntensity[i]; }
            }
            else if (_triggerColBounds.GetRelativePosition(col) == _disableWhenEntered)
            {
                _toggleStarted = true;
                for (int i = 0; i < _lightsCount; ++i) { _toggleInProcess[i] = true; _toIntensity[i] = _offIntensity[i]; }
            }
        }
    }
}
