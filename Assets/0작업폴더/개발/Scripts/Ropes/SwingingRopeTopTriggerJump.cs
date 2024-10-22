using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingRopeTopTriggerJump : MonoBehaviour
{
    [SerializeField] private SwingingRope _rope;
    [SerializeField] private bool _playerIsInRangeOnTriggerStay = true;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_rope.PlayerIsAttached && col.CompareTag("Player"))
        {
            _rope.DisconnectPlayer();
            PlayerLogic.SetPlayerXYPos(transform.position);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (FrameInputReader.FrameInput.InputDir.y < 0 && col.CompareTag("Player"))
        {
            _rope.PlayerIsInRange = _playerIsInRangeOnTriggerStay;
        }
    }
}
