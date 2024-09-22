using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverConnectedEmmisions : LeverConnectedObject
{
    [SerializeField] private SpriteRenderer[] _emmisionsToToggle;
    [SerializeField] private bool _initiallyTurnedOn = false;
    [SerializeField] private float _switchSpeed = 5;

    private bool _emmsTurnedOn = false;

    private int _emmsCount;
    private bool _toggleStarted;
    private bool[] _toggleInProcess;
    private Color[] _toEmmColor, _onEmmColor, _offEmmColor;

    private void Start()
    {
        _emmsTurnedOn = _initiallyTurnedOn;

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
        if (_initiallyTurnedOn)
        {
            for (int i = 0; i < _emmsCount; ++i) _emmisionsToToggle[i].material.SetColor("_EmissionColor", _onEmmColor[i]);
        }
        else
        {
            for (int i = 0; i < _emmsCount; ++i) _emmisionsToToggle[i].material.SetColor("_EmissionColor", _offEmmColor[i]);
        }
    }

    public override void ActivatedAction(bool enabledState)
    {
        _emmsTurnedOn = !_emmsTurnedOn;
        _toggleStarted = true;

        if (_emmsTurnedOn)
        {
            for (int i = 0; i < _emmsCount; ++i) { _toggleInProcess[i] = true; _toEmmColor[i] = _onEmmColor[i]; }
        }
        else // !_lightsTurnedOn
        {
            for (int i = 0; i < _emmsCount; ++i) { _toggleInProcess[i] = true; _toEmmColor[i] = _offEmmColor[i]; }
        }
    }

    private void Update()
    {
        if (_toggleStarted) toggleEmmisions();
    }

    private void toggleEmmisions()
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
}
