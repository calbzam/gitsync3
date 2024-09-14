using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderStopAutoClimb : MonoBehaviour
{
    [SerializeField] private LadderTrigger _ladder;
    private bool _autoClimbPrevState;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //if (PlayerLogic.PlayerRb.velocity.y < 0 && !PlayerLogic.Player.IsOnLadder)
            _autoClimbPrevState = _ladder.AutoClimbWhenJumpedOn;
            _ladder.AutoClimbWhenJumpedOn = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _ladder.AutoClimbWhenJumpedOn = _autoClimbPrevState;
        }
    }
}
