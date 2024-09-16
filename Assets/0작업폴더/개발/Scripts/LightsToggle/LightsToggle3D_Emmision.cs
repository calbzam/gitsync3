using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsToggle3D_Emmision : MonoBehaviour
{
    [SerializeField] private BoxCollider _triggerColToUse;
    private ColBounds3D _triggerColBounds;

    [SerializeField] private SpriteRenderer[] _emmisionsToToggle;
    [SerializeField] private float _switchSpeed = 5;
    
    private Collider _mainCameraCol;
    private bool _camIsInTrigger;

    private int _emmsCount;
    private bool _toggleStarted;
    private bool[] _toggleInProcess;
    private Color[] _toEmmColor, _onEmmColor, _offEmmColor;

    private bool setCamIsInTrigger()
    {
        return _camIsInTrigger = _triggerColBounds.OtherIsInSelf(_mainCameraCol);
    }

    private void Start()
    {
        _triggerColBounds = new ColBounds3D(_triggerColToUse);
        _mainCameraCol = Camera.main.GetComponentInChildren<Collider>();

        _toggleStarted = false;
        _emmsCount = _emmisionsToToggle.Length;

        _toggleInProcess = new bool[_emmsCount];
        _toEmmColor = new Color[_emmsCount];
        _onEmmColor = new Color[_emmsCount];
        _offEmmColor = new Color[_emmsCount];

        SetPresetEmmisions();
        EvalEmmsInitialStates();
    }

    private void SetPresetEmmisions()
    {
        for (int i = 0; i < _emmsCount; ++i)
        {
            _toggleInProcess[i] = false;
            _emmisionsToToggle[i].material.EnableKeyword("_EMISSION");
            _onEmmColor[i] = _emmisionsToToggle[i].material.GetColor("_EmissionColor");
            _offEmmColor[i] = Color.black;
        }
    }

    private void EvalEmmsInitialStates()
    {
        if (setCamIsInTrigger())
        {
            for (int i = 0; i < _emmsCount; ++i) _emmisionsToToggle[i].material.SetColor("_EmissionColor", _onEmmColor[i]);
        }
        else
        {
            for (int i = 0; i < _emmsCount; ++i) _emmisionsToToggle[i].material.SetColor("_EmissionColor", _offEmmColor[i]);
        }
    }

    private void Update()
    {
        if (!_camIsInTrigger)
        {
            if (setCamIsInTrigger())
            {
                _toggleStarted = true;
                for (int i = 0; i < _emmsCount; ++i) { _toggleInProcess[i] = true; _toEmmColor[i] = _onEmmColor[i]; }
            }
        }
        else // _camIsInTrigger
        {
            if (!setCamIsInTrigger())
            {
                _toggleStarted = true;
                for (int i = 0; i < _emmsCount; ++i) { _toggleInProcess[i] = true; _toEmmColor[i] = _offEmmColor[i]; }
            }
        }

        if (_toggleStarted) toggleLights();
    }

    private void toggleLights()
    {
        int cnt = 0;
        for (int i = 0; i < _emmsCount; ++i)
        {
            if (_toggleInProcess[i])
            {
                Color emmColor = _emmisionsToToggle[i].material.GetColor("_EmissionColor");
                emmColor = Vector4.MoveTowards(emmColor, _toEmmColor[i], _switchSpeed * Time.deltaTime);

                if (Vector4.Distance(emmColor, _toEmmColor[i]) < 0.001f)
                {
                    emmColor = _toEmmColor[i];
                    _toggleInProcess[i] = false;
                }
                _emmisionsToToggle[i].material.SetColor("_EmissionColor", emmColor);
            }
            else ++cnt;
        }

        if (cnt == _emmsCount) _toggleStarted = false;
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
