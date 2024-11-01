using UnityEngine;

public class LeverActivate : MonoBehaviour
{
    [SerializeField] private LeverConnectedObject[] _connectedObjects;

    [SerializeField] private Checkpoint _checkPointToActivate;
    [SerializeField] private bool _updateCheckpointOnActivate = false;

    [Header("Default (TransitionCameraTo.ZoomInTo) for LeverActivate is (ConnectedObjects[0])")]
    [SerializeField] private TransitionCameraTo _transitionCameraTo;

    public bool IsAutomatic = true;
    public bool NeedBattery = true;

    public bool IsActivated { get; private set; }

    private void Awake()
    {
        IsActivated = false;

        if (_checkPointToActivate != null)
            _checkPointToActivate.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (_transitionCameraTo.ZoomInTo == null)
            _transitionCameraTo.ZoomInTo = _connectedObjects[0].transform;
    }

    public void ToggleActivateBool()
    {
        IsActivated = !IsActivated;
    }

    public void ActivatedAction()
    {
        if (_transitionCameraTo != null) // transition camera to "VirtualCam - LeverActivated"
        {
            _transitionCameraTo.StartCameraTransition();

            PlayerLogic.LockPlayer();
            CanvasLogic.Letterboxes.Activateboxes(true);
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