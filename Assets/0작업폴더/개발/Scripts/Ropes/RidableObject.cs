using System;
using UnityEngine;
using Obi;
using UnityEngine.InputSystem;

public abstract class RidableObject : MonoBehaviour
{
    public abstract event Action<int, bool> PlayerOnThisObject;
    public bool _playerOnOtherObject = false;

    protected static InputControls _input;

    protected bool _playerIsAttached = false;
    protected bool _playerHasJumped = false;
    
    protected abstract void DisconnectPlayer(InputAction.CallbackContext ctx);

    protected virtual void Awake()
    {
        _input = new InputControls();
    }

    protected virtual void OnEnable()
    {
        _input.Enable();
        _input.Player.Jump.started += DisconnectPlayer;
    }

    protected virtual void OnDisable()
    {
        _input.Disable();
        _input.Player.Jump.started -= DisconnectPlayer;
    }
}
