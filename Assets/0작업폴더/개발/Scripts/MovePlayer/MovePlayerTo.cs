using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayerTo : MonoBehaviour
{
    [SerializeField] private Transform _toPoint;
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private bool _buttonPressRequired = true;
    [SerializeField] private FromDirection _fromDirection = FromDirection.DontCare;
    [SerializeField] private float _checkMargin = 0.02f;

    private enum FromDirection
    {
        DontCare,
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
    }

    private void OnEnable()
    {
        CentralInputReader.Input.Player.EnterHatchDoor.started += EnterButtonPressed;
    }

    private void OnDisable()
    {
        CentralInputReader.Input.Player.EnterHatchDoor.started -= EnterButtonPressed;
    }

    private void Update()
    {
        if (_isMovingPlayer)
        {
            PlayerLogic.SetPlayerXYPos(Vector2.MoveTowards(PlayerLogic.Player.transform.position, _toPoint.position, Time.deltaTime * _moveSpeed));

            if (MyMath.Vector2DiffLessThan(PlayerLogic.Player.transform.position, _toPoint.position, _checkMargin)) finishMovingPlayer();
        }
    }

    private void EnterButtonPressed(InputAction.CallbackContext ctx)
    {
        startMovingPlayer();
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
                PlayerLogic.Player.RespawnButtonAllowed = false;

                PlayerLogic.SetPlayerZPosition(_toPoint.position.z);
            }
        }
    }

    private void finishMovingPlayer()
    {
        _isMovingPlayer = false;
        PlayerLogic.Player.RespawnButtonAllowed = true;
        PlayerLogic.IgnorePlayerGroundCollision(false);
        PlayerLogic.FreePlayer();

        if (PlayerLogic.Player.IsOnLadder) PlayerLogic.Player.SetPlayerOnLadder(true);
    }

    private bool checkValidEnter()
    {
        switch (_fromDirection)
        {
            case FromDirection.DontCare: return true;
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _playerIsInRange = true;
            if (!_buttonPressRequired) startMovingPlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _playerIsInRange = false;
        }
    }
}
