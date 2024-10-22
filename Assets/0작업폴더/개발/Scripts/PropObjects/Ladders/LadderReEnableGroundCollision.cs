using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderReEnableGroundCollision : MonoBehaviour
{
    [SerializeField] private LadderTrigger _ladder;
    private bool _bypassGroundPrevState;

    private void Start()
    {
        _bypassGroundPrevState = _ladder.BypassGroundCollision;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (PlayerLogic.Player.CurrentLadder == _ladder)
            {
                _ladder.BypassGroundCollision = false;
                PlayerLogic.IgnorePlayerGroundCollision(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (PlayerLogic.Player.CurrentLadder == _ladder)
            {
                _ladder.BypassGroundCollision = _bypassGroundPrevState;
                if (PlayerLogic.Player.IsOnLadder)
                    PlayerLogic.IgnorePlayerGroundCollision(_bypassGroundPrevState);
            }
        }
    }
}
