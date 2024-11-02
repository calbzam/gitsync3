using UnityEngine;

public class LeverActivate : MonoBehaviour
{
    [SerializeField] private LeverConnectedObject[] _connectedObjects;

    [SerializeField] private Checkpoint _checkPointToActivate;
    [SerializeField] private bool _updateCheckpointOnActivate = false;

    public bool IsActivated { get; private set; }

    public bool IsAutomatic = true;
    public bool NeedBattery = true;

    [Header("Default (TransitionCameraTo.ZoomInTo) for LeverActivate is (ConnectedObjects[0])")]
    [SerializeField] private TransitionCameraTo _transitionCameraTo;
    
    private void Awake()
    {
        IsActivated = false;

        if (_checkPointToActivate != null)
            _checkPointToActivate.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (_transitionCameraTo != null)
        {
            if (_transitionCameraTo.ZoomInTo == null)
                _transitionCameraTo.ZoomInTo = _connectedObjects[0].transform;
        }
    }

    public void ToggleActivateBool()
    {
        IsActivated = !IsActivated;
    }

    public void ActivatedAction()
    {
        if (_transitionCameraTo != null) // transition camera to "VirtualCam - LeverActivated"
        {
            _transitionCameraTo.StartTransitionToDestCam();
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