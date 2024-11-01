using Cinemachine;
using UnityEngine;

public class TransitionCameraTo : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] private CinemachineVirtualCamera _toVirtualCam;
    public Transform ZoomInTo;
    [SerializeField] private DestCamMode _destinationCameraMode = DestCamMode.ZoomIntoObject;
    [SerializeField] private TransitionMode _camTransitionMode = TransitionMode.SwapToDestCamThenToMainCam;
    [SerializeField] private bool _activateLetterboxesOnTransition = true;

    [Header("Duration settings")]
    [SerializeField] private float _camTransitionDuration = 0.6f;
    private static float _camTransitionDurOrig;
    [SerializeField] private float _camTransitionHoldDuration = 1.3f;

    private bool _startCamTransition;
    private float _camTransitionStartTime;
    private float _camHoldEndTime;

    private bool _swapBackToMainCamCalled;

    private static CinemachineBrain _mainCamCinemachineBrain;
    private static CinemachineVirtualCamera _mainVirtualCam;
    private static CinemachineConfiner _toVirtualCamConfiner;

    private enum DestCamMode
    {
        ZoomIntoObject,
        DestCamStaysPut,
    }

    private enum TransitionMode
    {
        SwapToDestCamThenToMainCam,
        SwapToDestCamAndStay,
    }

    public void TransitionBackToDefaultCam()
    {
        _swapBackToMainCamCalled = true;
        SwapBackToDefaultCam();
    }

    public void StartTransitionToDestCam()
    {
        if (_startCamTransition) return; // transition already started

        _mainCamCinemachineBrain.m_DefaultBlend.m_Time = _camTransitionDuration;
        _mainVirtualCam.m_Transitions.m_InheritPosition = true;

        if (_destinationCameraMode == DestCamMode.ZoomIntoObject) _toVirtualCam.Follow = ZoomInTo;
        _toVirtualCamConfiner.m_BoundingShape2D = CameraBounds.VirtualCamDefaultConfiner.m_BoundingShape2D;
        _toVirtualCam.enabled = true;

        _camTransitionStartTime = Time.time;
        _startCamTransition = true;

        if (_camTransitionMode != TransitionMode.SwapToDestCamAndStay) PlayerLogic.LockPlayer();
        if (_activateLetterboxesOnTransition) CanvasLogic.Letterboxes.Activateboxes(true);
    }

    private void Awake()
    {
        _startCamTransition = false;
        _swapBackToMainCamCalled = false;
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
        EvalSwapBackToMainCam();
    }
    
    private void EvalSwapBackToMainCam()
    {
        if (!_swapBackToMainCamCalled && _camTransitionMode == TransitionMode.SwapToDestCamAndStay) return;

        if (_startCamTransition)
        {
            if (_toVirtualCam.enabled) // measure duration of looking at activated target
            {
                if (Time.time - _camTransitionStartTime > _camTransitionHoldDuration)
                {
                    SwapBackToDefaultCam();
                }
            }
            else // measure until LeverActivatedVirtualCam -> DefaultVirtualCam transition
            {
                if (Time.time - _camHoldEndTime > _camTransitionDuration + 0.1f)
                {
                    if (_swapBackToMainCamCalled) _swapBackToMainCamCalled = false;
                    EndSwapBackToDefaultCam();
                }
            }
        }
    }

    private void SwapBackToDefaultCam()
    {
        _toVirtualCam.enabled = false;
        _camHoldEndTime = Time.time;

        if (_activateLetterboxesOnTransition) CanvasLogic.Letterboxes.Activateboxes(false);
    }

    private void EndSwapBackToDefaultCam()
    {
        _mainCamCinemachineBrain.m_DefaultBlend.m_Time = _camTransitionDurOrig;
        _mainVirtualCam.m_Transitions.m_InheritPosition = false;
        _startCamTransition = false;

        PlayerLogic.FreePlayer();
    }
}
