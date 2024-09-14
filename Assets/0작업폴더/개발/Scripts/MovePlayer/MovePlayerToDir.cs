using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerToDir : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private FromDirection _fromDirection = FromDirection.FromBelow;
    private Vector3 _toDirection;
    [SerializeField] private float _checkMargin = 0.02f;

    private enum FromDirection
    {
        FromAbove,
        FromBelow,
        FromLeft,
        FromRight,
    }

    private bool _playerIsInRange;
    private bool _isMovingPlayer;

    private void Start()
    {
        _playerIsInRange = false;
        _isMovingPlayer = false;
        calcToDirection();
    }

    private void Update()
    {
        if (_isMovingPlayer)
        {
            PlayerLogic.Player.transform.position += Time.deltaTime * _moveSpeed * _toDirection;
        }
    }

    private void calcToDirection()
    {
        switch (_fromDirection)
        {
            default: _toDirection = Vector3.zero; break;

            case FromDirection.FromAbove:
                _toDirection = Vector3.down; break;

            case FromDirection.FromBelow:
                _toDirection = Vector3.up; break;

            case FromDirection.FromLeft:
                _toDirection = Vector3.right; break;

            case FromDirection.FromRight:
                _toDirection = Vector3.left; break;
        }
    }

    private bool checkValidEnter()
    {
        switch (_fromDirection)
        {
            default: return true;

            case FromDirection.FromAbove:
                return PlayerLogic.Player.transform.position.y > transform.position.y;

            case FromDirection.FromBelow:
                return PlayerLogic.Player.transform.position.y < transform.position.y;

            case FromDirection.FromLeft:
                return PlayerLogic.Player.transform.position.x < transform.position.x;

            case FromDirection.FromRight:
                return PlayerLogic.Player.transform.position.x > transform.position.x;
        }
    }

    private void startMovingPlayer()
    {
        if (_playerIsInRange && !PlayerLogic.PlayerIsLocked)
        {
            if (checkValidEnter())
            {
                PlayerLogic.LockPlayer();
                PlayerLogic.IgnorePlayerGroundCollision(true);
                _isMovingPlayer = true;
            }
        }
    }

    private void finishMovingPlayer()
    {
        _isMovingPlayer = false;
        PlayerLogic.IgnorePlayerGroundCollision(false);
        PlayerLogic.FreePlayer();

        if (PlayerLogic.Player.IsOnLadder) PlayerLogic.Player.SetPlayerOnLadder(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerIsInRange = true;
            startMovingPlayer();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerIsInRange = false;
            finishMovingPlayer();
        }
    }
}
