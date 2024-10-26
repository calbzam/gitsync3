using UnityEngine;

public class LightsToggle3D : MonoBehaviour
{
    [SerializeField] private BoxCollider _triggerColToUse;
    private ColBounds3D _triggerColBounds;
    [SerializeField] private Light[] _lightsToToggle;
    [SerializeField] private float[] _initalLightsIntensities;

    [Header("Initial delay (when you want it to not immediately turn on)")]
    [SerializeField] private bool evalLightsOnWhenEnabled = true;
    [SerializeField] private float _firstDelayDur = 0;
    private float _firstDelayTimer;
    private bool _firstDelayTimerActivated;
    [SerializeField] private float _switchSpeed = 5;

    [Header("Turn on/off when entering/exiting trigger")]
    [SerializeField] private bool _turnOnEnabled = true;
    [SerializeField] private bool _turnOffEnabled = true;

    private bool _inited = false;
    private bool _checkedInitIntensities;
    private bool _presetAllSet;

    private Collider _mainCameraCol;
    private bool _camIsInTrigger;

    private int _lightsCount;
    private bool _toggleStarted;
    private bool[] _toggleInProcess;
    private float[] _toIntensity, _onIntensity, _offIntensity;

    private bool checkCamIsInTrigger()
    {
        return _camIsInTrigger = _triggerColBounds.OtherIsInSelf(_mainCameraCol);
    }

    private bool checkPlayerIsInTrigger()
    {
        return _camIsInTrigger = _triggerColBounds.OtherIsInSelf(PlayerLogic.Player.transform);
    }

    private void OnEnable()
    {
        if (!_inited)
        {
            if (_firstDelayDur > 0.0001)
            {
                _firstDelayTimerActivated = true;
                _firstDelayTimer = Time.time;
            }

            _checkedInitIntensities = false;
            _presetAllSet = false;

            _triggerColBounds = new ColBounds3D(_triggerColToUse);
            _mainCameraCol = Camera.main.GetComponentInChildren<Collider>();

            _toggleStarted = false;
            _lightsCount = _lightsToToggle.Length;

            _toggleInProcess = new bool[_lightsCount];
            _toIntensity = new float[_lightsCount];
            _onIntensity = new float[_lightsCount];
            _offIntensity = new float[_lightsCount];

            CheckIfInitialIntensities();
            SetPresetIntensities();
            if (evalLightsOnWhenEnabled) EvalLightsInitialStates();
            _inited = true;
        }
    }

    private void CheckIfInitialIntensities()
    {
        for (int i = 0; i < _initalLightsIntensities.Length; ++i)
        {
            _onIntensity[i] = _initalLightsIntensities[i];
        }
        _checkedInitIntensities = true;
    }

    private void SetPresetIntensities()
    {
        bool noMoreInitialUpdates = true;
        for (int i = 0; i < _lightsCount; ++i)
        {
            if (_onIntensity[i] < _lightsToToggle[i].intensity) // not previously initialzed on first startup
            {
                _onIntensity[i] = _lightsToToggle[i].intensity;
                _offIntensity[i] = 0;
                _toggleInProcess[i] = false;
                noMoreInitialUpdates = false;
            }
        }
        if (noMoreInitialUpdates) _presetAllSet = true;
    }

    private void EvalLightsInitialStates()
    {
        if (checkCamIsInTrigger())
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
        //if (!_checkedInitIntensities) CheckIfInitialIntensities();
        //if (!_presetAllSet) SetPresetIntensities(); // update _onIntensity[i] until "max" intensities are reached
        if (!_inited) return;
        if (_firstDelayTimerActivated)
        {
            if (Time.time - _firstDelayTimer > _firstDelayDur)
                _firstDelayTimerActivated = false;
            return;
        }


        if (!_camIsInTrigger)
        {
            if (_turnOnEnabled && checkCamIsInTrigger())
            {
                _toggleStarted = true;
                for (int i = 0; i < _lightsCount; ++i) { _toggleInProcess[i] = true; _toIntensity[i] = _onIntensity[i]; }
            }
        }
        else // _camIsInTrigger
        {
            if (_turnOffEnabled && !checkCamIsInTrigger())
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
    //        _isToggling = true;
    //        for (int i = 0; i < _lightsCount; ++i) { _toggleInProcess[i] = true; _toIntensity[i] = _onIntensity[i]; }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("MainCamera Collider"))
    //    {
    //        _isToggling = true;
    //        for (int i = 0; i < _lightsCount; ++i) { _toggleInProcess[i] = true; _toIntensity[i] = _offIntensity[i]; }
    //    }
    //}
}
