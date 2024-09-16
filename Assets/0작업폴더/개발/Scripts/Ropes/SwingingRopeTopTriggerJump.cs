using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingRopeTopTriggerJump : MonoBehaviour
{
    [SerializeField] private SwingingRope _rope;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_rope.PlayerIsAttached && col.CompareTag("Player"))
        {
            _rope.DisconnectPlayer();
            PlayerLogic.SetPlayerXYPos(transform.position);
        }
    }
}
