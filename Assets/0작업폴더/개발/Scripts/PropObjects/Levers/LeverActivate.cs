using Cinemachine;
using UnityEngine;

public class LeverActivate : MonoBehaviour
{
    [SerializeField] private LeverConnectedObject[] _connectedObjects;
    [SerializeField] private Transform _replaceZoomInObjTo;

    [SerializeField] private Checkpoint _checkPointToActivate;
    [SerializeField] private bool _updateCheckpointOnActivate = false;

    private static CinemachineBrain _mainCamCinemachineBrain;
    private static CinemachineVirtualCamera _defaultVirtualCam;
    private static CinemachineVirtualCamera _leverActivatedVirtualCam;
    private static CinemachineConfiner _leverActivatedVirtualCamConfiner;

    public bool IsAutomatic = true;
    public bool NeedBattery = true;

    public bool IsActivated { get; private set; }

    [Header("Camera transition to first connected object")] // TO DO: move these parts to another script later
    [SerializeField] private bool _transitionCamOnActivate = true;
    [SerializeField] private float _camTransitionDuration = 0.6f;
    private static float _camTransitionDurOrig;
    [SerializeField] private float _camTransitionHoldDuration = 1.3f;
    private bool _startCamTransition;
    private float _camTransitionStartTime;
    private float _camHoldEndTime;

    private void Awake()
    {
        IsActivated = false;
        _startCamTransition = false;

        if (_checkPointToActivate != null)
            _checkPointToActivate.gameObject.SetActive(false);
    }

    private void Start()
    {
        _mainCamCinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        _camTransitionDurOrig = _mainCamCinemachineBrain.m_DefaultBlend.m_Time;

        _defaultVirtualCam = GameObject.FindGameObjectWithTag("DefaultVirtualCam").GetComponent<CinemachineVirtualCamera>();
        _defaultVirtualCam.m_Transitions.m_InheritPosition = false;

        _leverActivatedVirtualCam = GameObject.FindGameObjectWithTag("LeverActivatedVirtualCam").GetComponent<CinemachineVirtualCamera>();
        _leverActivatedVirtualCamConfiner = _leverActivatedVirtualCam.GetComponent<CinemachineConfiner>();
        _leverActivatedVirtualCam.enabled = false;
    }

    private void Update()
    {
        EvalCamTransition();
    }

    private void EvalCamTransition()
    {
        if (_startCamTransition)
        {
            if (_leverActivatedVirtualCam.enabled) // measure duration of looking at activated target
            {
                if (Time.time - _camTransitionStartTime > _camTransitionHoldDuration)
                {
                    _leverActivatedVirtualCam.enabled = false;
                    _camHoldEndTime = Time.time;
                }
            }
            else // measure until LeverActivatedVirtualCam -> DefaultVirtualCam transition
            {
                if (Time.time - _camHoldEndTime > _camTransitionDuration)
                {
                    _mainCamCinemachineBrain.m_DefaultBlend.m_Time = _camTransitionDurOrig;
                    _defaultVirtualCam.m_Transitions.m_InheritPosition = false;
                    _startCamTransition = false;

                    PlayerLogic.FreePlayer();
                }
            }
        }
    }

    public void ToggleActivateBool()
    {
        IsActivated = !IsActivated;
    }

    public void ActivatedAction()
    {
        if (_transitionCamOnActivate)
        {
            _mainCamCinemachineBrain.m_DefaultBlend.m_Time = _camTransitionDuration;
            _defaultVirtualCam.m_Transitions.m_InheritPosition = true;

            _leverActivatedVirtualCam.Follow = (_replaceZoomInObjTo != null) ? _replaceZoomInObjTo.transform : _connectedObjects[0].transform;
            _leverActivatedVirtualCamConfiner.m_BoundingShape2D = CameraBounds.VirtualCamDefaultConfiner.m_BoundingShape2D;
            _leverActivatedVirtualCam.enabled = true;

            _camTransitionStartTime = Time.time;
            _startCamTransition = true;

            PlayerLogic.LockPlayer();
        }

        foreach (var obj in _connectedObjects) obj.ActivatedAction(IsActivated);
    }

    public void UpdateCheckpoint()
    {
        if (_checkPointToActivate != null)
        {
            _checkPointToActivate.gameObject.SetActive(true);

            if (_updateCheckpointOnActivate)
                PlayerLogic.Player.SetRespawnPoint(_checkPointToActivate);
        }
    }
}