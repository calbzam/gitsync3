using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverActivate : MonoBehaviour
{
    [SerializeField] private LeverConnectedObject _connectedObject;
    [SerializeField] private Checkpoint _checkPointToActivate;

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

    public void BatteryInsertedAction()
    {
        _checkPointToActivate.gameObject.SetActive(true);
        PlayerLogic.Player.SetRespawnPos(_checkPointToActivate.RespawnPoint.position);
    }
}