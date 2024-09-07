using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverActivate : MonoBehaviour
{
    [SerializeField] private LeverConnectedObject _connectedObject;
    [SerializeField] private Checkpoint _checkPointToActivate;
    [SerializeField] private bool _updateCheckpointOnActivate = false;

    public bool IsAutomatic;

    public bool IsActivated { get; private set; }
    
    private void Start()
    {
        IsActivated = false;

        if (_checkPointToActivate != null)
            _checkPointToActivate.gameObject.SetActive(false);
    }

    public void ToggleActivate()
    {
        IsActivated = !IsActivated;
    }

    public void ActivatedAction()
    {
        _connectedObject.ActivatedAction(IsActivated);
    }

    public void UpdateCheckpoint()
    {
        _checkPointToActivate.gameObject.SetActive(true);

        if (_updateCheckpointOnActivate)
            PlayerLogic.Player.SetRespawnPos(_checkPointToActivate.RespawnPoint.position);
    }
}