using System;
using UnityEngine;

public abstract class RidableObject : MonoBehaviour
{
    public abstract event Action<int, bool> PlayerOnThisObject;
    public bool PlayerOnOtherObject = false; // visible in Unity Editor but resets to false on Start call

    public bool PlayerIsAttached { get; protected set; }
    protected bool _playerHasJumped;
    
    public abstract void DisconnectPlayer();

    protected virtual void Start()
    {
        PlayerOnOtherObject = false;
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
