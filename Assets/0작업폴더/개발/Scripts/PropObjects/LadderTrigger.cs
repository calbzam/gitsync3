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
    public Vector2 Direction { get; private set; }
    public void UpdateLadderDirection() => Direction = (TopPoint.position - BottomPoint.position).normalized;

    public bool AutoClimbWhenJumpedOn { get; set; }
    public bool BypassGroundCollision { get; set; }

    public float ClimbSpeed { get; set; }
    public float StepSize { get; set; }

    public bool PlayerIsOnLadder { get; set; }
    public bool PlayerIsInRange { get; set; }

    private void Start()
    {
        PlayerIsOnLadder = false;
        PlayerIsInRange = false;
        AutoClimbWhenJumpedOn = _ladderSettings.AutoClimbWhenJumpedOn;
        BypassGroundCollision = _ladderSettings.BypassGroundCollision;
        ClimbSpeed = _ladderSettings.ClimbSpeed;
        StepSize = _ladderSettings.StepSize;
        UpdateLadderDirection();
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
        if (PlayerLogic.Player.LadderClimbAllowed)
            JumpFromLadder();
    }

    public void JumpFromLadder()
    {
        if (PlayerIsOnLadder)
        {
            PlayerLogic.Player.SetPlayerOnLadder(false, this);
            PlayerLogic.Player.SetPlayerInLadderRange(this);
            PlayerLogic.Player.JumpingFromLadder = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerLogic.Player.SetPlayerInLadderRange(this);

            if (AutoClimbWhenJumpedOn)
                if (!PlayerLogic.Player.JumpingFromLadder && !PlayerLogic.Player.OnGround)
                {
                    PlayerLogic.Player.SetPlayerOnLadder(true, this);
                }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (PlayerLogic.Player.ZPosSetToGround) // runs only once
            if (col.gameObject.CompareTag("Player"))
            {
                PlayerLogic.SetPlayerZPosition(transform.position.z - 0.1f);
                PlayerLogic.Player.ZPosSetToGround = false;
            }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (PlayerLogic.Player.CurrentLadder == this)
                PlayerLogic.Player.JumpingFromLadder = false; // player sufficiently away from ladder

            PlayerLogic.Player.SetPlayerOnLadder(false, this);
        }
    }
}
