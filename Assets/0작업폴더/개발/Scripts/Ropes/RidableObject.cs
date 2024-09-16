using System;
using UnityEngine;

public abstract class RidableObject : MonoBehaviour
{
    public abstract event Action<int, bool> PlayerOnThisObject;
    public bool _playerOnOtherObject = false;

    public bool PlayerIsAttached { get; protected set; }
    protected bool _playerHasJumped;
    
    public abstract void DisconnectPlayer();

    protected virtual void Start()
    {
        PlayerIsAttached = false;
        _playerHasJumped = false;
    }

    protected virtual void OnEnable()
    {
        FrameInputReader.JumpPressed += DisconnectPlayer;
    }

    protected virtual void OnDisable()
    {
        FrameInputReader.JumpPressed -= DisconnectPlayer;
    }
}
