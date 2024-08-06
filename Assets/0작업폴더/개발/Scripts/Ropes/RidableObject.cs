using System;
using UnityEngine;
using Obi;
using UnityEngine.InputSystem;

public abstract class RidableObject : MonoBehaviour
{
    public abstract event Action<int, bool> PlayerOnThisObject;
    public bool _playerOnOtherObject = false;

    protected bool _playerIsAttached = false;
    protected bool _playerHasJumped = false;
    
    protected abstract void DisconnectPlayer();

    protected virtual void OnEnable()
    {
        FrameInputReader.JumpPressed += DisconnectPlayer;
    }

    protected virtual void OnDisable()
    {
        FrameInputReader.JumpPressed -= DisconnectPlayer;
    }
}
