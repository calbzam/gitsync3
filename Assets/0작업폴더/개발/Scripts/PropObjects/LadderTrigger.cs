using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderTrigger : MonoBehaviour
{
    [SerializeField] private Transform _topPoint;
    [SerializeField] private Transform _bottomPoint;

    [Header("공중에서 사다리에 닿으면 자동으로 사다리 탑승")]
    public bool AutoClimbWhenJumpedOn = true;

    [Header("사다리에 타고 있을 때는 땅과의 충돌 무시")]
    public bool BypassGroundCollision = true;

    public Transform TopPoint => _topPoint; // for public access
    public Transform BottomPoint => _bottomPoint;

    public float ClimbSpeed { get; set; } = 0.05f;
    public float StepSize { get; set; } = 0.5f;
    public float StepProgress { get; set; }

    private bool _jumpingFromLadder;

    private void Start()
    {
        _jumpingFromLadder = false;
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
            _jumpingFromLadder = true;
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
            if (!PlayerLogic.Player.IsOnLadder && !_jumpingFromLadder && !PlayerLogic.Player.OnGround)
                PlayerLogic.Player.SetPlayerOnLadder(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerLogic.Player.IsInLadderRange = false;
            PlayerLogic.Player.SetPlayerOnLadder(false);
            _jumpingFromLadder = false; // player sufficiently away from ladder
        }
    }
}
