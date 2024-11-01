using Cinemachine;
using UnityEngine;

public class TransitionCameraTo : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] private CinemachineVirtualCamera _toVirtualCam;
    public Transform ZoomInTo;

    [Header("Duration settings")]
    [SerializeField] private float _camTransitionDuration = 0.6f;
    private static float _camTransitionDurOrig;
    [SerializeField] private float _camTransitionHoldDuration = 1.3f;

    private bool _startCamTransition;
    private float _camTransitionStartTime;
    private float _camHoldEndTime;

    private static CinemachineBrain _mainCamCinemachineBrain;
    private static CinemachineVirtualCamera _mainVirtualCam;
    private static CinemachineConfiner _toVirtualCamConfiner;

    public void StartCameraTransition()
    {
        _mainCamCinemachineBrain.m_DefaultBlend.m_Time = _camTransitionDuration;
        _mainVirtualCam.m_Transitions.m_InheritPosition = true;

        _toVirtualCam.Follow = ZoomInTo;
        //_toVirtualCam.Follow = (ZoomInTo != null) ? ZoomInTo.transform : _connectedObjects[0].transform;
        _toVirtualCamConfiner.m_BoundingShape2D = CameraBounds.VirtualCamDefaultConfiner.m_BoundingShape2D;
        _toVirtualCam.enabled = true;

        _camTransitionStartTime = Time.time;
        _startCamTransition = true;
    }

    private void Awake()
    {
        _startCamTransition = false;
    }

    private void Start()
    {
        _mainCamCinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        _camTransitionDurOrig = _mainCamCinemachineBrain.m_DefaultBlend.m_Time;

        _mainVirtualCam = GameObject.FindGameObjectWithTag(Tags.DefaultVirtualCamTag).GetComponent<CinemachineVirtualCamera>();
        _mainVirtualCam.m_Transitions.m_InheritPosition = false;

        _toVirtualCamConfiner = _toVirtualCam.GetComponent<CinemachineConfiner>();
        _toVirtualCam.enabled = false;
    }

    private void Update()
    {
        EvalCamTransition();
    }
    
    private void EvalCamTransition()
    {
        if (_startCamTransition)
        {
            if (_toVirtualCam.enabled) // measure duration of looking at activated target
            {
                if (Time.time - _camTransitionStartTime > _camTransitionHoldDuration)
                {
                    _toVirtualCam.enabled = false;
                    _camHoldEndTime = Time.time;

                    CanvasLogic.Letterboxes.Activateboxes(false);
                }
            }
            else // measure until LeverActivatedVirtualCam -> DefaultVirtualCam transition
            {
                if (Time.time - _camHoldEndTime > _camTransitionDuration + 0.1f)
                {
                    _mainCamCinemachineBrain.m_DefaultBlend.m_Time = _camTransitionDurOrig;
                    _mainVirtualCam.m_Transitions.m_InheritPosition = false;
                    _startCamTransition = false;

                    PlayerLogic.FreePlayer();
                }
            }
        }
    }

}
