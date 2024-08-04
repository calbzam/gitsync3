using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderStopAutoClimb : MonoBehaviour
{
    [SerializeField] private LadderTrigger _ladder;
    private bool _autoClimbPrevState;

    private void Start()
    {
        _autoClimbPrevState = _ladder.AutoClimbWhenJumpedOn;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (PlayerLogic.Player.CurrentLadder == _ladder)
                //if (PlayerLogic.PlayerRb.velocity.y < 0 && !PlayerLogic.Player.IsOnLadder)
                    _ladder.AutoClimbWhenJumpedOn = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (PlayerLogic.Player.CurrentLadder == _ladder)
                _ladder.AutoClimbWhenJumpedOn = _autoClimbPrevState;
        }
    }
}
