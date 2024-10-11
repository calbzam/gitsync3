using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// refer to Test_ColorParticleCollisions.cs

public class GroundHolderRope : RopeParticleCollisionReaderBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerEnteredRope += Self_PlayerEnteredRope;
        PlayerExitedRope += Self_PlayerExitedRope;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerEnteredRope -= Self_PlayerEnteredRope;
        PlayerExitedRope -= Self_PlayerExitedRope;
    }

    //private void Update()
    //{
    //    Debug.Log(ParticleHasCollision.Count(x => x == true) + ", " + PlayerIsOnRope);
    //}

    private void Self_PlayerEnteredRope()
    {
        PlayerLogic.Player.GroundCheckAllowed = false;
        PlayerLogic.Player.OnGround = false;
        CentralInputReader.Input.Player.Jump.started += Self_JumpStartedAction;
    }

    private void Self_PlayerExitedRope()
    {
        PlayerLogic.Player.GroundCheckAllowed = true;
        CentralInputReader.Input.Player.Jump.started -= Self_JumpStartedAction;
    }

    private void Self_JumpStartedAction(InputAction.CallbackContext ctx)
    {
        PlayerLogic.Player.ExecuteJump();
    }
}
