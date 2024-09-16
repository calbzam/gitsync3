using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceXToPlayer : MonoBehaviour
{
    [SerializeField] private float _addForceAmount = 40;
    [SerializeField] private float _playerMaxSpeedX = 18;
    [SerializeField] private Dir _moveDir = Dir.ToRight;

    private enum Dir { ToLeft = -1, ToRight = 1, }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerLogic.Player.LimitXVelocity) PlayerLogic.Player.LimitXVelocity = false;
            PlayerLogic.Player.Rb.AddForce(_addForceAmount * (int)_moveDir * Vector2.right, ForceMode2D.Force);
            PlayerLogic.Player.LimitXVelocityTo(_playerMaxSpeedX);
        }
    }
}
