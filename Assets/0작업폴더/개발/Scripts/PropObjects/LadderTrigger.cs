using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderTrigger : MonoBehaviour
{
    [SerializeField] private LadderSettings _ladderSettings;
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;

    public Transform TopPoint => _topPoint; // for public access
    public Transform BottomPoint => _bottomPoint;

    public bool AutoClimbWhenJumpedOn { get; set; }
    public bool BypassGroundCollision { get; set; }

    public float ClimbSpeed { get; set; }
    public float StepSize { get; set; }
    public float StepProgress { get; set; }

    public bool JumpingFromLadder { get; set; }

    private void Start()
    {
        JumpingFromLadder = false;
        AutoClimbWhenJumpedOn = _ladderSettings.AutoClimbWhenJumpedOn;
        BypassGroundCollision = _ladderSettings.BypassGroundCollision;
        ClimbSpeed = _ladderSettings.ClimbSpeed;
        StepSize = _ladderSettings.StepSize;
    }

    private void OnEnable()
    {
        CentralInputReader.Input.Player.Jump.started += JumpStartedAction;
    }

    private void OnDisable()
    {
        CentralInputReader.Input.Player.Jump.started -= JumpStartedAction;
    }

    private void JumpStartedAction(InputAction.CallbackContext ctx)
    {
        JumpFromLadder();
    }

    public void JumpFromLadder()
    {
        if (PlayerLogic.Player.IsOnLadder && PlayerLogic.Player.CurrentLadder == this)
        {
            PlayerLogic.Player.SetPlayerOnLadder(false);
            JumpingFromLadder = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerLogic.Player.IsInLadderRange = true;
            PlayerLogic.Player.CurrentLadder = this;
        }

        if (AutoClimbWhenJumpedOn)
            if (!PlayerLogic.Player.IsOnLadder && !JumpingFromLadder && !PlayerLogic.Player.OnGround)
                PlayerLogic.Player.SetPlayerOnLadder(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerLogic.Player.IsInLadderRange = false;
            PlayerLogic.Player.SetPlayerOnLadder(false);
            JumpingFromLadder = false; // player sufficiently away from ladder
        }
    }
}
