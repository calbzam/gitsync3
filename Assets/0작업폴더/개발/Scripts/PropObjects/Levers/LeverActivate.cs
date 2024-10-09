using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverActivate : MonoBehaviour
{
    [SerializeField] private LeverConnectedObject[] _connectedObjects;
    [SerializeField] private Checkpoint _checkPointToActivate;
    [SerializeField] private bool _updateCheckpointOnActivate = false;

    public bool IsAutomatic = true;
    public bool NeedBattery = true;

    public bool IsActivated { get; private set; }
    
    private void Awake()
    {
        IsActivated = false;

        if (_checkPointToActivate != null)
            _checkPointToActivate.gameObject.SetActive(false);
    }

    public void ToggleActivateBool()
    {
        IsActivated = !IsActivated;
    }

    public void ActivatedAction()
    {
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