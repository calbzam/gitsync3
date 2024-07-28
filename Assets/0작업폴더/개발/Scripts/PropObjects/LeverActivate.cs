using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverActivate : MonoBehaviour
{
    [SerializeField] private LeverConnectedObject _connectedObject;

    public bool IsActivated { get; private set; }

    private void Start()
    {
        IsActivated = false;
    }

    public void ToggleActivate()
    {
        IsActivated = !IsActivated;
        _connectedObject.ActivatedAction(IsActivated);
    }
}